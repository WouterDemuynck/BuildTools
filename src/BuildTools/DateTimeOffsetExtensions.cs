//
// Copyright (c) 2008-2012 Wouter Demuynck
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;

namespace BuildTools
{
	internal static class DateTimeOffsetExtensions
	{
		public static int YearsSince(this DateTimeOffset me, DateTimeOffset other)
		{
			// FIX: Only use the date components of the dates.
			me = me.Date;
			other = other.Date;

			// TODO: Throw decent error message from resources.
			if (other > me) 
				throw new ArgumentException("The other date must be earlier than the current date.", "other");

			int years = me.Year - other.Year;
			if (me.Month < other.Month) years--;

			return years;
		}

		public static int MonthsSince(this DateTimeOffset me, DateTimeOffset other)
		{
			// FIX: Only use the date components of the dates.
			me = me.Date;
			other = other.Date;

			if (other > me) 
				throw new ArgumentException("The other date must be earlier than the current date.", "other");

			int months = 0;
			int years = me.YearsSince(other);

			if (me.Month < other.Month) months = (me.Month + 12) - other.Month;
			else months = me.Month - other.Month;

			months += years * 12;
			return months;
		}

		public static int DaysSince(this DateTimeOffset me, DateTimeOffset other)
		{
			// FIX: Only use the date components of the dates.
			me = me.Date;
			other = other.Date;

			if (other.Ticks > me.Ticks) 
				throw new ArgumentException("The other date must be earlier than the current date.", "other");

			return me.Subtract(other).Days;
		}
	}
}