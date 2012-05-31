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
	/// Enumeration containing all available revision number generation algorithms.
	/// </summary>
	public enum RevisionNumberType
	{
		/// <summary>
		/// The revision number is fixed an thus left unchanged.
		/// </summary>
		Fixed,
		/// <summary>
		/// The revision number is incremented.
		/// </summary>
		Increment,
		/// <summary>
		/// The revision number is incremented if the build number has not changed; otherwise it is reset to 0.
		/// </summary>
		BuildIncrement,
		/// <summary>
		/// The revision number is calculated by formatting the current time using the 'HHmm' format.
		/// </summary>
		HourMinute,
		/// <summary>
		/// The revision number is calculated by dividing the number of seconds passed since midnight by 10. 
		/// </summary>
		DaySecond,
		/// <summary>
		/// The revision number is calculated by multiplying the number of seconds passed since midnight with
		/// the maximum allowed value of the revision number (65535) divided by the number of seconds in a day (86400).
		/// The result is a revision number that changes roughly every 1.3 seconds.
		/// </summary>
		DayFraction
	}
}