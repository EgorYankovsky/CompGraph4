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
   private readonly GLWpfControl _control;
   
   private readonly Color _backGroundColor = Color.White;
           
   private readonly Color _meshColor = Color.LightGray;

   private readonly Color _axisColor = Color.Gray;

   private readonly Color _pointColor = Color.Black;

   private readonly Color _splineColor = Color.DarkRed;

   private readonly Color _derivativeColor = Color.DarkGreen;

   private readonly Color _primalColor = Color.DarkBlue;

   public readonly double ZoomIn = 1.25;

   public readonly double ZoomOut = 1.25;

   private readonly int _cellSize = 1;

   private readonly double xl, yl;

   public int DrawMode { get; set; }

   private double Xm { get { return xl + _control.ActualWidth; } }
   private double Ym { get { return yl + _control.ActualHeight; } }

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
      GL.Begin(PrimitiveType.Lines);

      for (int i = (int)(xl - xl % _cellSize); i < Xm + Xm % _cellSize; i += _cellSize)
      {
         GL.Vertex2(i, yl);
         GL.Vertex2(i, Ym);
      }

      for (int i = (int)(yl - yl % _cellSize); i < Ym + Ym % _cellSize; i += _cellSize)
      {
         GL.Vertex2(xl, i);
         GL.Vertex2(Xm, i);
      }
      GL.End();
   }


   public void DrawAxis()
   {
      GL.LineWidth(3);
      GL.Color3(_axisColor);
      GL.Begin(PrimitiveType.Lines);

      GL.Vertex2(xl, 0);
      GL.Vertex2(Xm, 0);

      GL.Vertex2(0, yl);
      GL.Vertex2(0, Ym);
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
