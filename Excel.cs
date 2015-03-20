/////////////////////////////////////////////////////////////////////////////
// $Id:$
//
// Copyright (c) 2006-2015 by James John McGuire
// All rights reserved.
/////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////
// Namespace includes
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Globalization;
using Common.Logging;
using Microsoft.Office.Interop.Excel;

namespace Zenware.Common.UtilsNet
{
	// Represents a Excel object.
	public class ExcelWrapper
	{
		private uint columnCount = 0;
		private Microsoft.Office.Interop.Excel.Application excelApplication = null;
		private _Workbook m_ExcelWorkBook = null;
		private Worksheet m_ExcelWorkSheet = null;
		private Sheets m_ExcelWorkSheets = null;
		private string m_FileName = string.Empty;
		private static readonly ILog log = LogManager.GetLogger
			(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		//private string m_Version = string.Empty;

		[CLSCompliantAttribute(false)]
		public uint ColumnCount
		{
			get { return columnCount; }
			set { columnCount = value; }
		}

		public string FileName
		{
			get { return m_FileName; }
			set { m_FileName = value; }
		}

		public ExcelWrapper()
		{
			excelApplication = new Microsoft.Office.Interop.Excel.Application();
			//m_Version = excelApplication.Version;

			excelApplication.DisplayAlerts = false;
		}

		~ExcelWrapper()
		{
			excelApplication.Quit();
		}

		public void CloseFile()
		{
			m_ExcelWorkBook.Close(false, null, false);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public string OpenFile()
		{
			try
			{
				if (!string.IsNullOrEmpty(m_FileName))
				{
					m_ExcelWorkBook = excelApplication.Workbooks.Open(
						m_FileName,
						0,
						true,
						1,
						true,
						System.Reflection.Missing.Value,
						System.Reflection.Missing.Value,
						true,
						System.Reflection.Missing.Value,
						true,
						System.Reflection.Missing.Value,
						false,
						System.Reflection.Missing.Value,
						false,
						false);
				}
			}
			catch (Exception e)
			{
				this.CloseFile();
				return e.Message;
			}
			return "OK";
		}

		public string OpenFile(string fileName)
		{
			try
			{
				if (!string.IsNullOrEmpty(fileName))
				{
					m_FileName = fileName;
					m_ExcelWorkBook = excelApplication.Workbooks.Open(
						fileName,
						0,
						false,
						1,
						true,
						System.Reflection.Missing.Value,
						System.Reflection.Missing.Value,
						true,
						System.Reflection.Missing.Value,
						true,
						System.Reflection.Missing.Value,
						false,
						System.Reflection.Missing.Value,
						false,
						false);
				}
			}
			catch (Exception e)
			{
				this.CloseFile();
				return e.Message;
			}
			return "OK";
		}

		public void Delete(int rowId, int columnId)
		{
			string Range = columnId.ToString(CultureInfo.CurrentCulture) + 
				rowId.ToString(CultureInfo.CurrentCulture);

			Range workingRangeCells = m_ExcelWorkSheet.get_Range(Range, Type.Missing);

			workingRangeCells.Delete(XlDeleteShiftDirection.xlShiftUp);
		}

		public void DeleteRow(int rowId)
		{
			string Range = "A" + rowId + ":IM" + rowId;

			Range workingRangeCells = m_ExcelWorkSheet.get_Range(Range, Type.Missing);

			System.Array array = (System.Array)workingRangeCells.Cells.Value2;
			string[] arrayS = ConvertToStringArray(array);
			log.Info(CultureInfo.InvariantCulture, m =>
				m("Range: {0}", arrayS[2]));

			workingRangeCells.Delete(XlDeleteShiftDirection.xlShiftUp);
		}

		public void GetExcelSheets()
		{
			if (m_ExcelWorkBook != null)
			{
				m_ExcelWorkSheets = m_ExcelWorkBook.Worksheets;
			}
		}

		public bool FindExcelWorksheet(string worksheetName)
		{
			bool bSheetFound = false;

			if (m_ExcelWorkSheets != null)
			{
				// Step thru the worksheet collection and see if the sheet is
				// available. If found return true;
				for (int i = 1; i <= m_ExcelWorkSheets.Count; i++)
				{
					m_ExcelWorkSheet = (Worksheet)m_ExcelWorkSheets.get_Item((object)i);
					if (m_ExcelWorkSheet.Name.Equals(worksheetName))
					{
						// Get method interface
						_Worksheet _sheet = (_Worksheet)m_ExcelWorkSheet;
						_sheet.Activate();
						bSheetFound = true;
						break;
					}
				}
			}
			return bSheetFound;
		}

		public string[] GetRange(string range)
		{
			Range workingRangeCells = m_ExcelWorkSheet.get_Range(range, Type.Missing);

			System.Array array = (System.Array)workingRangeCells.Cells.Value2;
			string[] arrayS = ConvertToStringArray(array);

			return arrayS;
		}

		public string[] GetRow(int idRow)
		{
			string[] Row = new string[columnCount];
			string Range = "A" + idRow + ":IM" + idRow;

			Row = GetRange(Range);
			return Row;
		}

		public void Save()
		{
			//m_ExcelWorkBook.Save();
			m_ExcelWorkBook.SaveAs(m_FileName, System.Reflection.Missing.Value,
				null, null, false, false, XlSaveAsAccessMode.xlExclusive,
				XlSaveAsAccessMode.xlExclusive,
				System.Reflection.Missing.Value, System.Reflection.Missing.Value,
				System.Reflection.Missing.Value, System.Reflection.Missing.Value);
		}

		[CLSCompliantAttribute(false)]
		public void SetCell(uint row, uint column, string value)
		{
			m_ExcelWorkSheet.Cells[row + 2, column + 1] = value;
		}

		private static string[] ConvertToStringArray(System.Array values)
		{
			string[] newArray = new string[values.Length];

			int index = 0;
			for (int i = values.GetLowerBound(0); i <= values.GetUpperBound(0); i++)
			{
				for (int j = values.GetLowerBound(1); j <= values.GetUpperBound(1); j++)
				{
					if (values.GetValue(i, j) == null)
					{
						newArray[index] = "";
					}
					else
					{
						newArray[index] = (string)values.GetValue(i, j).ToString();
					}
					index++;
				}
			}
			return newArray;
		}
	}
}