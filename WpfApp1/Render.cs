using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Wpf;

namespace WpfApp1;

public class Render
{
   private GLWpfControl _control;
   
   private Color _backGroundColor = Color.White;

   private Color _meshColor = Color.FloralWhite;

   private Color _pointColor = Color.Black;

   private Color _splineColor = Color.DarkRed;

   private Color _derivativeColor = Color.DarkGreen;

   private Color _primalColor = Color.DarkBlue;

   public readonly double ZoomIn = 1.25;

   public readonly double ZoomOut = 1.25;

   private int _cellSize = 1;

   private double xl, yl;

   public int DrawMode { get; set; }

   // Check mistake.
   private double xm { get { return xl + _control.ActualWidth; } }
   private double ym { get { return yl + _control.ActualHeight; } }

   public double Scale = 30f;
   public double TranslateX = 0;
   public double TranslateY = 0;


   public Render(GLWpfControl glControl)
   {
      _control = glControl;

      xl = -0.5 * _control.ActualWidth;
      yl = -0.5 * _control.ActualHeight;

      // начальная позиция холста
      TranslateX = 0.5 * _control.ActualWidth / Scale;
      TranslateY = -0.5 * _control.ActualHeight / Scale;
   }


   public void Initialize()
   {
      GL.ClearColor(_backGroundColor);

      GL.Viewport(0, 0, (int)_control.ActualWidth, (int)_control.ActualHeight);
      
      GL.MatrixMode(MatrixMode.Projection);
      GL.LoadIdentity();

      GL.Ortho(0, _control.ActualWidth, -_control.ActualHeight, 0, -1, 1);

      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();
   }

   public void ApplyResize()
   {
      Initialize();
   }

   public void Refresh()
   {
      _control.InvalidateVisual();
   }

   public void ScaleIn(int delta = 1)
   {
      Scale *= (float)Math.Pow(ZoomIn, delta);
   }

   public void ScaleOut(int delta = 1)
   {
      if (Scale > 2)
         Scale *= (float)Math.Pow(ZoomOut, delta);
   }

   public void Move(int dx, int dy)
   {
      TranslateX -= dx / Scale;
      TranslateY -= dy / Scale;
   }


   public void Begin()
   {
      GL.Clear(ClearBufferMask.ColorBufferBit);

      GL.LoadIdentity();
      GL.Scale(Scale, Scale, 1);
      GL.Translate(TranslateX, TranslateY, 0);
   }

   public void End()
   {
      GL.PopMatrix();
   }

   public void DrawGrid()
   {
      GL.LineWidth(1);
      GL.Color3(_meshColor);
      GL.Begin(PrimitiveType.Points);

      for (int i = (int)(xl - xl % _cellSize); i < xm + xm % _cellSize; i += _cellSize)
      {
         GL.Vertex2(i, yl);
         GL.Vertex2(i, ym);
      }

      for (int i = (int)(yl - yl % _cellSize); i < ym + ym % _cellSize; i += _cellSize)
      {
         GL.Vertex2(i, yl);
         GL.Vertex2(i, ym);
      }
      GL.End();
   }


   public void DrawAxis()
   {
      GL.LineWidth(3);
      GL.Color3(_meshColor);
      GL.Begin(PrimitiveType.Points);

      GL.Vertex2(xl, 0);
      GL.Vertex2(xm, 0);

      GL.Vertex2(0, yl);
      GL.Vertex2(0, ym);
      GL.End();
   }

   public void DrawSpline(Spline s)
   {
      GL.LineWidth(2);
      if ((DrawMode & (1 << 0))  != 0)
      {
         GL.Color3(_splineColor);
         GL.Begin(PrimitiveType.LineStrip);
         foreach (var pt in s.PointsView)
         {
            GL.Vertex2(pt.X, pt.Y);
         }
         GL.End();
      }

      GL.LineWidth(2);
      if ((DrawMode & (1 << 1)) != 0)
      {
         GL.Color3(_derivativeColor);
         GL.Begin(PrimitiveType.LineStrip);
         foreach (var pt in s.d_points)
         {
            GL.Vertex2(pt.X, pt.Y);
         }
         GL.End();
      }

      GL.LineWidth(2);
      if ((DrawMode & (1 << 2)) != 0)
      {
         GL.Color3(_primalColor);
         GL.Begin(PrimitiveType.LineStrip);
         foreach (var pt in s.i_points)
         {
            GL.Vertex2(pt.X, pt.Y);
         }
         GL.End();
      }
   }

   public void DrawPoints(Spline s)
   {
      GL.Color3(_pointColor);
      GL.Begin(PrimitiveType.Quads);
      foreach (var pt in s.Points)
      {
         GL.Vertex2(pt.X - 0.1, pt.Y + 0.1);
         GL.Vertex2(pt.X - 0.1, pt.Y - 0.1);
         GL.Vertex2(pt.X + 0.1, pt.Y - 0.1);
         GL.Vertex2(pt.X + 0.1, pt.Y + 0.1);
      }
      GL.End();
   }

}
