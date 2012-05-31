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

namespace BuildTools
{
	/// <summary>
	/// Enumeration containing all available build number generation algorithms.
	/// </summary>
	public enum BuildNumberType
	{
		/// <summary>
		/// The build number is fixed and thus left unchanged.
		/// </summary>
		Fixed,
		/// <summary>
		/// The build number is incremented.
		/// </summary>
		Increment,
		/// <summary>
		/// The build number is calculated by prepending the number of years since the
		/// starting date of the calculation to the current date formatted using the 'MMdd' format.
		/// The algorithm corrects the build number automatically when the number of years would cause
		/// an overflow.
		/// </summary>
		YearMonthDay,
		/// <summary>
		/// The build number is calculated by prepending the number of months since the starting
		/// date of the calculation to the day of the current month.
		/// </summary>
		MonthDay,
		/// <summary>
		/// The build number is calculated by prepending the number of years since the starting date
		/// of the calculation to the day of the current year.
		/// </summary>
		BuildDay
	}
}