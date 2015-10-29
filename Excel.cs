/////////////////////////////////////////////////////////////////////////////
// $Id$
//
// Copyright (c) 2006-2015 by James John McGuire
// All rights reserved.
/////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////
// Namespace includes
/////////////////////////////////////////////////////////////////////////////
using Common.Logging;
using Microsoft.Office.Interop.Excel;
using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace DigitalZenWorks.Common.Utils
{
	public enum Formats
	{
		Date,
		General,
		Text
	}

	// Represents a Excel object.
	public class ExcelWrapper
	{
		private int columnCount = 0;
		private Microsoft.Office.Interop.Excel.Application excelApplication =
			null;
		private string filename = string.Empty;
		private bool hasHeaderRow = false;
		private static readonly ILog log = LogManager.GetLogger
			(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private Workbook workBook = null;
		private Worksheet workSheet = null;
		private Sheets workSheets = null;
		//private string version = string.Empty;

		public int ColumnCount
		{
			get { return columnCount; }
			set { columnCount = value; }
		}

		public string FileName
		{
			get { return filename; }
			set { filename = value; }
		}

		public bool HasHeaderRow
		{
			get { return hasHeaderRow; }
			set { hasHeaderRow = value; }
		}

		public int LastColumnUsed
		{
			get
			{
				int lastUsedColumn = -1;

				if (null != workSheet)
				{
					Range last = workSheet.Cells.SpecialCells(
						XlCellType.xlCellTypeLastCell, Type.Missing);

					lastUsedColumn = last.Column;
				}

				return lastUsedColumn;
			}
		}

		public int LastRowUsed
		{
			get
			{
				int lastUsedRow = -1;

				if (null != workSheet)
				{
					Range last = workSheet.Cells.SpecialCells(
						XlCellType.xlCellTypeLastCell, Type.Missing);

					lastUsedRow = last.Row;

					// Excel uses a 1 based index
					lastUsedRow--;
				}

				return lastUsedRow;
			}
		}

		public ExcelWrapper()
		{
			excelApplication = new Microsoft.Office.Interop.Excel.Application();
			//version = excelApplication.Version;

			excelApplication.DisplayAlerts = false;
		}

		public void Close()
		{
			CloseFile();
			if (excelApplication != null)
			{
				excelApplication.Quit();
				Marshal.ReleaseComObject(excelApplication);
				excelApplication = null;
			}
		}

		public void CloseFile()
		{
			if (workSheet != null)
			{
				Marshal.ReleaseComObject(workSheet);
				workSheet = null;
			}
			if (workSheets != null)
			{
				Marshal.ReleaseComObject(workSheets);
				workSheets = null;
			}
			if (workBook != null)
			{
				workBook.Close(false, null, false);
				Marshal.ReleaseComObject(workBook);
				workBook = null;
			}
		}

		public Workbook Create()
		{
			workBook = excelApplication.Workbooks.Add();
			workSheets = workBook.Worksheets;

			workSheet = workSheets.Add();

			return workBook;
		}

		public void Delete(int row, int column,
			XlDeleteShiftDirection direction)
		{
			Range range = GetCell(row, column);

			range.Delete(direction);
			Marshal.ReleaseComObject(range);
		}

		public void DeleteRow(int row)
		{
			Range range = GetRange(row, row, 0, LastColumnUsed);

			range.Delete(XlDeleteShiftDirection.xlShiftUp);
			Marshal.ReleaseComObject(range);
		}

		public bool FindExcelWorksheet(string worksheetName)
		{
			bool sheetFound = false;

			if (workSheets != null)
			{
				// Step through the worksheet collection and see if the sheet
				// is available. If found return true;
				for (int index = 1; index <= workSheets.Count; index++)
				{
					workSheet = (Worksheet)workSheets.get_Item((object)index);
					if (workSheet.Name.Equals(worksheetName))
					{
						// Get method interface
						_Worksheet _sheet = (_Worksheet)workSheet;
						_sheet.Activate();
						sheetFound = true;
						break;
					}
				}
			}

			return sheetFound;
		}

		public Range GetCell(int row, int column)
		{
			row = AdjustRow(row);
			if (column < int.MaxValue)
			{
				// excel is 1 based
				column++;
			}

			Range range = workSheet.Cells[row, column];

			return range;
		}

		public double GetCellBackgroundColor(int row, int column)
		{
			Range range = GetCell(row, column);

			double color = range.Interior.Color;

			return color;
		}

		public int GetCellBackgroundColorIndex(int row, int column)
		{
			Range range = GetCell(row, column);

			int color = range.Interior.ColorIndex;

			return color;
		}

		public double GetCellFontColor(int row, int column)
		{
			Range range = GetCell(row, column);

			double color = range.Font.Color;

			return color;
		}

		public int GetCellFontColorIndex(int row, int column)
		{
			Range range = GetCell(row, column);

			int color = range.Font.ColorIndex;

			return color;
		}

		public string GetCellValue(int row, int column)
		{
			string cellValue = null;
			Range cell = GetCell(row, column);

			if (null != cell.Value2)
			{
				cellValue = cell.Value2.ToString();
			}

			return cellValue;
		}

		public Range GetColumnRange(int columnNumber)
		{
			Range range = GetRange(0, LastRowUsed, columnNumber, columnNumber);

			range = range.EntireColumn;

			return range;
		}

		public int GetCountNonemptyCells(Range range)
		{
			double result = excelApplication.WorksheetFunction.CountA(range);

			return Convert.ToInt32(result);
		}

		public static string GetExcelColumnName(int columnNumber)
		{
			int dividend = columnNumber;
			string columnName = String.Empty;
			int modulo;

			while (dividend > 0)
			{
				modulo = (dividend - 1) % 26;
				columnName =
					Convert.ToChar(65 + modulo).ToString() + columnName;
				dividend = (int)((dividend - modulo) / 26);
			}

			return columnName;
		}

		public void GetExcelSheets()
		{
			if (workBook != null)
			{
				workSheets = workBook.Worksheets;
			}
		}

		public Range GetRange(int rowBegin, int rowEnd, int columnBegin,
			int columnEnd)
		{
			// excel is 1 based
			rowBegin = AdjustRow(rowBegin);
			rowEnd = AdjustRow(rowEnd);
			if (columnBegin < int.MaxValue)
			{
				columnBegin++;
			}

			string columnBeginName = GetExcelColumnName(columnBegin);
			string columnEndName = GetExcelColumnName(columnEnd);

			string rangeQuery =
				columnBeginName + rowBegin + ":" + columnEndName + rowEnd;

			Range range =
				workSheet.get_Range(rangeQuery, Type.Missing);

			return range;
		}

		public string[] GetRangeValues(int rowBegin, int rowEnd,
			int columnBegin, int columnEnd)
		{
			Range range = GetRange(rowBegin, rowEnd, columnBegin, columnEnd);

			string[] stringArray =
				GetStringArray(range.Cells.Value2);

			Marshal.ReleaseComObject(range);

			return stringArray;
		}

		public string[] GetRowValues(int rowId)
		{
			int lastUsedColumn = LastColumnUsed;

			Range range = GetRange(rowId, rowId, 0, lastUsedColumn);

			string[][] rows = GetStringArray(range.Cells.Value2);
			string[] row = rows[0];

			Marshal.ReleaseComObject(range);

			return row;
		}

		public Range GetRow(int rowId)
		{
			int lastUsedColumn = LastColumnUsed;

			Range range = GetRange(rowId, rowId, 0, lastUsedColumn);

			return range;
		}

		public bool IsCellEmpty(int row, int column)
		{
			bool empty = false;

			string contents = GetCellValue(row, column);

			if (string.IsNullOrWhiteSpace(contents))
			{
				empty = true;
			}

			return empty;
		}

		public bool OpenFile()
		{
			return OpenFile(filename);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
			"CA1031:DoNotCatchGeneralExceptionTypes")]
		public bool OpenFile(string fileName)
		{
			bool result = false;

			try
			{
				if (!string.IsNullOrEmpty(fileName))
				{
					filename = fileName;
					workBook = excelApplication.Workbooks.Open(fileName, 0,
						false, 1, true, System.Reflection.Missing.Value,
						System.Reflection.Missing.Value, true,
						System.Reflection.Missing.Value, true,
						System.Reflection.Missing.Value, false,
						System.Reflection.Missing.Value, false, false);

					result = true;
				}
			}
			catch (Exception ex)
			{
				this.CloseFile();
				log.Error(CultureInfo.InvariantCulture,
					m => m("Initialization Error: {0}", ex.Message));
			}

			return result;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
			"CA1031:DoNotCatchGeneralExceptionTypes")]
		public void Save()
		{
			try
			{
				workBook.SaveAs(filename, XlFileFormat.xlWorkbookDefault,
					null, null, false, false, XlSaveAsAccessMode.xlExclusive,
					XlSaveAsAccessMode.xlExclusive,
					System.Reflection.Missing.Value,
					System.Reflection.Missing.Value,
					System.Reflection.Missing.Value,
					System.Reflection.Missing.Value);
			}
			catch (Exception ex)
			{
				this.CloseFile();
				log.Error(CultureInfo.InvariantCulture,
					m => m("Initialization Error: {0}", ex.Message));
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
			"CA1031:DoNotCatchGeneralExceptionTypes")]
		public void SaveAsCsv(string filename)
		{
			try
			{
				workBook.SaveAs(filename, XlFileFormat.xlCSVWindows);
			}
			catch (Exception ex)
			{
				this.CloseFile();
				log.Error(CultureInfo.InvariantCulture,
					m => m("Initialization Error: {0}", ex.Message));
			}
		}

		public void SetBackgroundColor(int row, int column, double color)
		{
			Range range = GetCell(row, column);
			range.Interior.Color = color;

			Marshal.ReleaseComObject(range);
		}

		public void SetCell(int row, int column, string value)
		{
			Range cell = GetCell(row, column);

			cell.Value = value;
		}

		public void SetColumnFormat(int column, Formats format)
		{
			Range columnRange = GetColumnRange(column);

			switch(format)
			{
				case Formats.Date:
				{
					columnRange.NumberFormat = "yyyy-mm-dd";
					break;
				}
				case Formats.Text:
				{
					columnRange.NumberFormat = "@";
					break;
				}
				default:
				{
					break;
				}
			}

			Marshal.ReleaseComObject(columnRange);
		}

		public void SetFontColor(int row, int column, Color color)
		{
			Range range = GetCell(row, column);
			range.Font.Color = System.Drawing.ColorTranslator.ToOle(color);

			Marshal.ReleaseComObject(range);
		}

		public void SetFontColor(int row, int column, double color)
		{
			Range range = GetCell(row, column);
			range.Font.Color = color;

			Marshal.ReleaseComObject(range);
		}

		public void SetTextFormat(int row, int column)
		{
			Range range = GetCell(row, column);
			range.NumberFormat = "@";

			Marshal.ReleaseComObject(range);
		}

		/// <summary>
		/// Excel uses a 1 based index. Programs using this, use a 0 based
		/// index, so need to adjust.  Also, compensate, if there is a header.
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		private int AdjustRow(int row)
		{
			row++;

			if (true == hasHeaderRow)
			{
				row++;
			}

			return row;
		}

		private static string[][] GetStringArray(Object rangeValues)
		{
			string[][] stringArray = null;

			Array array = rangeValues as Array;
			if (null != array)
			{
				int rank = array.Rank;
				if (rank > 1)
				{
					int rowCount = array.GetLength(0);
					int columnCount = array.GetUpperBound(1);

					stringArray = new string[rowCount][];

					for (int index = 0; index < rowCount; index++)
					{
						stringArray[index] = new string[columnCount];

						for (int index2 = 0; index2 < columnCount; index2++)
						{
							Object obj = array.GetValue(index + 1, index2 + 1);
							if (null != obj)
							{
								string value = obj.ToString();

								stringArray[index][index2] = value;
							}
						}
					}
				}
			}

			return stringArray;
		}

	}
}
