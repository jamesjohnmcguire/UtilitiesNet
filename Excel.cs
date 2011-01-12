/////////////////////////////////////////////////////////////////////////////
// $Id: Excel.cs $
//
// Represents a Excel object.
//
// Copyright @2010 by James John McGuire (DigitalZenWorks)
// All rights reserved.
/////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////
// Namespace includes
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Office.Interop.Excel;

namespace Zenware.Common.UtilsNet
{
	public class Excel
	{
		private uint m_ColumnCount = 0;
		private _Application m_ExcelApplication = null;
		private _Workbook m_ExcelWorkBook = null;
		private Worksheet m_ExcelWorkSheet = null;
		private Sheets m_ExcelWorkSheets = null;
		private string m_FileName = string.Empty;
		private string m_Version = string.Empty;

		public uint ColumnCount
		{
			get { return m_ColumnCount; }
			set { m_ColumnCount = value; }
		}

		public string FileName
		{
			get { return m_FileName; }
			set { m_FileName = value; }
		}

		public Excel()
		{
			m_ExcelApplication = new ApplicationClass();
			m_Version = m_ExcelApplication.Version;

			m_ExcelApplication.DisplayAlerts = false;

		}

		~Excel()
		{
			m_ExcelApplication.Quit();
		}

		public void CloseFile()
		{
			m_ExcelWorkBook.Close(false, null, false);
		}

		public string OpenFile()
		{
			try
			{
				if (!string.IsNullOrEmpty(m_FileName))
				{
					m_ExcelWorkBook = m_ExcelApplication.Workbooks.Open(
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

		public string OpenFile(string FileName)
		{
			try
			{
				if (!string.IsNullOrEmpty(FileName))
				{
					m_FileName = FileName;
					m_ExcelWorkBook = m_ExcelApplication.Workbooks.Open(
						FileName,
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

		public void GetExcelSheets()
		{
			if (m_ExcelWorkBook != null)
			{
				m_ExcelWorkSheets = m_ExcelWorkBook.Worksheets;
			}
		}

		public bool FindExcelWorksheet(string WorkSheetName)
		{
			bool bSheetFound = false;

			if (m_ExcelWorkSheets != null)
			{
				// Step thru the worksheet collection and see if the sheet is
				// available. If found return true;
				for (int i = 1; i <= m_ExcelWorkSheets.Count; i++)
				{
					m_ExcelWorkSheet = (Worksheet)m_ExcelWorkSheets.get_Item((object)i);
					if (m_ExcelWorkSheet.Name.Equals(WorkSheetName))
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

		public string[] GetRange(string Range)
		{
			Range workingRangeCells = m_ExcelWorkSheet.get_Range(Range, Type.Missing);

			System.Array array = (System.Array)workingRangeCells.Cells.Value2;
			string[] arrayS = this.ConvertToStringArray(array);

			return arrayS;
		}

		public string[] GetRow(int idRow)
		{
			string[] Row = new string[m_ColumnCount];
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

		public void SetCell(uint Row, uint Column, string Value)
		{
			m_ExcelWorkSheet.Cells[Row+2, Column+1] = Value;
		}

		private string[] ConvertToStringArray(System.Array values)
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
