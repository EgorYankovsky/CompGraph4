using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1;

public class SplineFunction
{
   public float A = 0.0f;
   public float B = 0.0f;
   public float C = 0.0f;
   public float D = 0.0f;
   public float H = 0.0f;
   public float X = 0.0f;

   public SplineFunction()
   {

   }

   public SplineFunction(float a, float b, float c, float d, float h, float x)
   {
      A = a;
      B = b;
      C = c;
      D = d;
      H = h;
      X = x;
   }

   public float F()
   {
      return F(X);
   }

   public float F(float s)
   {
      float ret = A;
      ret += B * (s - X);
      ret += C * (s - X) * (s - X);
      ret += D * (s - X) * (s - X) * (s - X);
      return ret;
   }

   public float D_F(float s)
   {
      float res = 0.0f;
      res += B;
      res += 2 * C * (s - X);
      res += 3 * D * (s - X) * (s - X);
      return res;
   }

   public float I_F(float s)
   {
      float res = A * (s - X);
      res += 0.5f * B * (s - X) * (s - X);
      res += 0.333333f * C * (s - X) * (s - X) * (s - X);
      res += 0.25f * D * (s - X) * (s - X) * (s - X) * (s - X);
      return res;
   }
}
