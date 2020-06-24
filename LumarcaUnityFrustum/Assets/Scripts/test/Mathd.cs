// Type: UnityEngine.Mathd
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\Program Files (x86)\Unity\Editor\Data\Managed\UnityEngine.dll
using System;
using System.Runtime.CompilerServices;

namespace UnityEngine {
	public struct Mathd {
		public const float PI = 3.141593f;
		public const float Infinity = float.PositiveInfinity;
		public const float NegativeInfinity = float.NegativeInfinity;
		public const float Deg2Rad = 0.01745329f;
		public const float Rad2Deg = 57.29578f;
		public const float Epsilon = 1.401298E-45f;
		
		public static float Sin(float d) {
			return Mathf.Sin(d);
		}
		
		public static float Cos(float d) {
			return Mathf.Cos(d);
		}
		
		public static float Tan(float d) {
			return Mathf.Tan(d);
		}
		
		public static float Asin(float d) {
			return Mathf.Asin(d);
		}
		
		public static float Acos(float d) {
			return Mathf.Acos(d);
		}
		
		public static float Atan(float d) {
			return Mathf.Atan(d);
		}
		
		public static float Atan2(float y, float x) {
			return Mathf.Atan2(y, x);
		}
		
		public static float Sqrt(float d) {
			return Mathf.Sqrt(d);
		}
		
		public static float Abs(float d) {
			return Mathf.Abs(d);
		}
		
		public static int Abs(int value) {
			return Mathf.Abs(value);
		}
		
		public static float Min(float a, float b) {
			if (a < b)
				return a;
			else
				return b;
		}
		
		public static float Min(params float[] values) {
			int length = values.Length;
			if (length == 0)
				return 0.0f;
			float num = values[0];
			for (int index = 1; index < length; ++index) {
				if (values[index] < num)
					num = values[index];
			}
			return num;
		}
		
		public static int Min(int a, int b) {
			if (a < b)
				return a;
			else
				return b;
		}
		
		public static int Min(params int[] values) {
			int length = values.Length;
			if (length == 0)
				return 0;
			int num = values[0];
			for (int index = 1; index < length; ++index) {
				if (values[index] < num)
					num = values[index];
			}
			return num;
		}
		
		public static float Max(float a, float b) {
			if (a > b)
				return a;
			else
				return b;
		}
		
		public static float Max(params float[] values) {
			int length = values.Length;
			if (length == 0)
				return 0f;
			float num = values[0];
			for (int index = 1; index < length; ++index) {
				if ((float)values[index] > (float)num)
					num = values[index];
			}
			return num;
		}
		
		public static int Max(int a, int b) {
			if (a > b)
				return a;
			else
				return b;
		}
		
		public static int Max(params int[] values) {
			int length = values.Length;
			if (length == 0)
				return 0;
			int num = values[0];
			for (int index = 1; index < length; ++index) {
				if (values[index] > num)
					num = values[index];
			}
			return num;
		}
		
		public static float Pow(float d, float p) {
			return Mathf.Pow(d, p);
		}
		
		public static float Exp(float power) {
			return Mathf.Exp(power);
		}
		
		public static float Log(float d, float p) {
			return Mathf.Log(d, p);
		}
		
		public static float Log(float d) {
			return Mathf.Log(d);
		}
		
		public static float Log10(float d) {
			return Mathf.Log10(d);
		}
		
		public static float Ceil(float d) {
			return (float)Math.Ceiling(d);
		}
		
		public static float Floor(float d) {
			return Mathf.Floor(d);
		}
		
		public static float Round(float d) {
			return Mathf.Round(d);
		}
		
		public static int CeilToInt(float d) {
			return (int)Math.Ceiling(d);
		}
		
		public static int FloorToInt(float d) {
			return (int)Mathf.Floor(d);
		}
		
		public static int RoundToInt(float d) {
			return (int)Mathf.Round(d);
		}
		
		public static float Sign(float d) {
			return d >= 0.0 ? 1f : -1f;
		}
		
		public static float Clamp(float value, float min, float max) {
			if (value < min)
				value = min;
			else if (value > max)
				value = max;
			return value;
		}
		
		public static int Clamp(int value, int min, int max) {
			if (value < min)
				value = min;
			else if (value > max)
				value = max;
			return value;
		}
		
		public static float Clamp01(float value) {
			if (value < 0.0)
				return 0.0f;
			if (value > 1.0)
				return 1f;
			else
				return value;
		}
		
		public static float Lerp(float from, float to, float t) {
			return from + (to - from) * Mathd.Clamp01(t);
		}
		
		public static float LerpAngle(float a, float b, float t) {
			float num = Mathd.Repeat(b - a, 360f);
			if (num > 180.0f)
				num -= 360f;
			return a + num * Mathd.Clamp01(t);
		}
		
		public static float MoveTowards(float current, float target, float maxDelta) {
			if (Mathd.Abs(target - current) <= maxDelta)
				return target;
			else
				return current + Mathd.Sign(target - current) * maxDelta;
		}
		
		public static float MoveTowardsAngle(float current, float target, float maxDelta) {
			target = current + Mathd.DeltaAngle(current, target);
			return Mathd.MoveTowards(current, target, maxDelta);
		}
		
		public static float SmoothStep(float from, float to, float t) {
			t = Mathd.Clamp01(t);
			t = (-2.0f * t * t * t + 3.0f * t * t);
			return to * t + from * (1.0f - t);
		}
		
		public static float Gamma(float value, float absmax, float gamma) {
			bool flag = false;
			if (value < 0.0)
				flag = true;
			float num1 = Mathd.Abs(value);
			if (num1 > absmax) {
				if (flag)
					return -num1;
				else
					return num1;
			} else {
				float num2 = Mathd.Pow(num1 / absmax, gamma) * absmax;
				if (flag)
					return -num2;
				else
					return num2;
			}
		}
		
		public static bool Approximately(float a, float b) {
			return Mathd.Abs(b - a) < Mathd.Max(1E-06f * Mathd.Max(Mathd.Abs(a), Mathd.Abs(b)), 1.121039E-44f);
		}
		
		public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed) {
			float deltaTime = (float)Time.deltaTime;
			return Mathd.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}
		
		public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime) {
			float deltaTime = Time.deltaTime;
			float maxSpeed = float.PositiveInfinity;
			return Mathd.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}
		
		public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime) {
			smoothTime = Mathd.Max(0.0001f, smoothTime);
			float num1 = 2f / smoothTime;
			float num2 = num1 * deltaTime;
			float num3 = (1.0f / (1.0f + num2 + 0.479999989271164f * num2 * num2 + 0.234999999403954f * num2 * num2 * num2));
			float num4 = current - target;
			float num5 = target;
			float max = maxSpeed * smoothTime;
			float num6 = Mathd.Clamp(num4, -max, max);
			target = current - num6;
			float num7 = (currentVelocity + num1 * num6) * deltaTime;
			currentVelocity = (currentVelocity - num1 * num7) * num3;
			float num8 = target + (num6 + num7) * num3;
			if (num5 - current > 0.0 == num8 > num5) {
				num8 = num5;
				currentVelocity = (num8 - num5) / deltaTime;
			}
			return num8;
		}
		
		public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed) {
			float deltaTime = (float)Time.deltaTime;
			return Mathd.SmoothDampAngle(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}
		
		public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime) {
			float deltaTime = (float)Time.deltaTime;
			float maxSpeed = float.PositiveInfinity;
			return Mathd.SmoothDampAngle(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}
		
		public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime) {
			target = current + Mathd.DeltaAngle(current, target);
			return Mathd.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}
		
		public static float Repeat(float t, float length) {
			return t - Mathd.Floor(t / length) * length;
		}
		
		public static float PingPong(float t, float length) {
			t = Mathd.Repeat(t, length * 2f);
			return length - Mathd.Abs(t - length);
		}
		
		public static float InverseLerp(float from, float to, float value) {
			if (from < to) {
				if (value < from)
					return 0f;
				if (value > to)
					return 1f;
				value -= from;
				value /= to - from;
				return value;
			} else {
				if (from <= to)
					return 0f;
				if (value < to)
					return 1f;
				if (value > from)
					return 0f;
				else
					return (1.0f - (value - to) / (from - to));
			}
		}
		
		public static float DeltaAngle(float current, float target) {
			float num = Mathd.Repeat(target - current, 360f);
			if (num > 180.0d)
				num -= 360f;
			return num;
		}
		
		internal static bool LineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, ref Vector2 result) {
			float num1 = p2.x - p1.x;
			float num2 = p2.y - p1.y;
			float num3 = p4.x - p3.x;
			float num4 = p4.y - p3.y;
			float num5 = num1 * num4 - num2 * num3;
			if (num5 == 0.0d)
				return false;
			float num6 = p3.x - p1.x;
			float num7 = p3.y - p1.y;
			float num8 = (num6 * num4 - num7 * num3) / num5;
			result = new Vector2(p1.x + num8 * num1, p1.y + num8 * num2);
			return true;
		}
		
		internal static bool LineSegmentIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, ref Vector2 result) {
			float num1 = p2.x - p1.x;
			float num2 = p2.y - p1.y;
			float num3 = p4.x - p3.x;
			float num4 = p4.y - p3.y;
			float num5 = (num1 * num4 - num2 * num3);
			if (num5 == 0.0d)
				return false;
			float num6 = p3.x - p1.x;
			float num7 = p3.y - p1.y;
			float num8 = (num6 * num4 - num7 * num3) / num5;
			if (num8 < 0.0d || num8 > 1.0d)
				return false;
			float num9 = (num6 * num2 - num7 * num1) / num5;
			if (num9 < 0.0d || num9 > 1.0d)
				return false;
			result = new Vector2(p1.x + num8 * num1, p1.y + num8 * num2);
			return true;
		}
	}
}