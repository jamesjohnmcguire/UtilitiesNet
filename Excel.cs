﻿/////////////////////////////////////////////////////////////////////////////
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
	// Represents a Excel object.
	public class ExcelWrapper
	{
		private uint columnCount = 0;

		private Microsoft.Office.Interop.Excel.Application excelApplication =
			null;

		private string filename = string.Empty;

		private static readonly ILog log = LogManager.GetLogger
			(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private Workbook workBook = null;
		private Worksheet workSheet = null;
		private Sheets workSheets = null;
		//private string version = string.Empty;

		[CLSCompliantAttribute(false)]
		public uint ColumnCount
		{
			get { return columnCount; }
			set { columnCount = value; }
		}

		public string FileName
		{
			get { return filename; }
			set { filename = value; }
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

		public void Delete(int rowId, int columnId)
		{
			string range = columnId.ToString(CultureInfo.InvariantCulture) +
				rowId.ToString(CultureInfo.InvariantCulture);

			Range workingRangeCells = workSheet.get_Range(range, Type.Missing);

			workingRangeCells.Delete(XlDeleteShiftDirection.xlShiftUp);
			Marshal.ReleaseComObject(workingRangeCells);
		}

		public void DeleteRow(int rowId)
		{
			int lastUsedColumn = LastColumnUsed;
			string lastColumn = GetExcelColumnName(lastUsedColumn);

			string range = "A" + rowId + ":" + lastColumn + rowId;

			Range workingRangeCells = workSheet.get_Range(range, Type.Missing);

			workingRangeCells.Delete(XlDeleteShiftDirection.xlShiftUp);
			Marshal.ReleaseComObject(workingRangeCells);
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

		public string GetCell(uint row, uint column)
		{
			object rangeObject = workSheet.Cells[row, column];
			Range range = (Range)rangeObject;
			object rangeValue = range.Value2;
			string cellValue = rangeValue.ToString();

			return cellValue;
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

		public Range GetRangeObject(string range)
		{
			Range workingRangeCells = workSheet.get_Range(range, Type.Missing);

			return workingRangeCells;
		}

		public string[] GetRange(string range)
		{
			Range workingRangeCells = workSheet.get_Range(range, Type.Missing);

			System.Array array = (System.Array)workingRangeCells.Cells.Value2;
			string[] stringArray = ConvertToStringArray(array);

			Marshal.ReleaseComObject(workingRangeCells);

			return stringArray;
		}

		public string[] GetRow(int rowId)
		{
			int lastUsedColumn = LastColumnUsed;
			string lastColumn = GetExcelColumnName(lastUsedColumn);

			string range = "A" + rowId + ":" + lastColumn + rowId;
			string[] row = GetRange(range);

			return row;
		}

		public Range GetRowRange(int rowId)
		{
			int lastUsedColumn = LastColumnUsed;
			string lastColumn = GetExcelColumnName(lastUsedColumn);

			string range = "A" + rowId + ":" + lastColumn + rowId;
			Range workingRangeCells = workSheet.get_Range(range, Type.Missing);

			return workingRangeCells;
		}

		public bool IsCellEmpty(uint row, uint column)
		{
			bool empty = false;

			string contents = GetCell(row, column);

			if (string.IsNullOrWhiteSpace(contents))
			{
				empty = true;
			}

			return empty;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
			"CA1031:DoNotCatchGeneralExceptionTypes")]
		public string OpenFile()
		{
			return OpenFile(filename);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
			"CA1031:DoNotCatchGeneralExceptionTypes")]
		public string OpenFile(string fileName)
		{
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
				}
			}
			catch (Exception ex)
			{
				this.CloseFile();
				log.Error(CultureInfo.InvariantCulture,
					m => m("Initialization Error: {0}", ex.Message));
				return ex.Message;
			}
			return "OK";
		}

		public void Save()
		{
			//workBook.Save();
			workBook.SaveAs(filename, System.Reflection.Missing.Value,
				null, null, false, false, XlSaveAsAccessMode.xlExclusive,
				XlSaveAsAccessMode.xlExclusive,
				System.Reflection.Missing.Value,
				System.Reflection.Missing.Value,
				System.Reflection.Missing.Value,
				System.Reflection.Missing.Value);
		}

		[CLSCompliantAttribute(false)]
		public void SetBackgroundColor(uint row, uint column, Color color)
		{
			string rangeQuery = column + row + ":" + column + row;

			Range range = GetRangeObject(rangeQuery);
			range.Interior.Color = System.Drawing.ColorTranslator.ToOle(color);
			Save();

			Marshal.ReleaseComObject(range);
		}

		[CLSCompliantAttribute(false)]
		public void SetCell(uint row, uint column, string value)
		{
			if ((row < (uint.MaxValue - 1)) && (column < uint.MaxValue))
			{
				workSheet.Cells[row + 2, column + 1] = value;
			}
			else
			{
				if (row >= (uint.MaxValue - 1))
				{
					throw new ArgumentOutOfRangeException("row",
					"row greater than Uint32.MaxValue");
				}
				if (column >= uint.MaxValue)
				{
					throw new ArgumentOutOfRangeException("column",
					"column greater than Uint32.MaxValue");
				}
			}
		}

		[CLSCompliantAttribute(false)]
		public void SetFontColor(uint row, uint column, Color color)
		{
			string rangeQuery = column + row + ":" + column + row;

			Range range = GetRangeObject(rangeQuery);
			range.Font.Color = System.Drawing.ColorTranslator.ToOle(color);
			Save();

			Marshal.ReleaseComObject(range);
		}

		private static string[] ConvertToStringArray(System.Array values)
		{
			string[] newArray = new string[values.Length];

			int index = 0;
			for (int i = values.GetLowerBound(0);
				i <= values.GetUpperBound(0); i++)
			{
				for (int j = values.GetLowerBound(1);
					j <= values.GetUpperBound(1); j++)
				{
					if (values.GetValue(i, j) == null)
					{
						newArray[index] = "";
					}
					else
					{
						newArray[index] =
							(string)values.GetValue(i, j).ToString();
					}
					index++;
				}
			}
			return newArray;
		}
	}
}