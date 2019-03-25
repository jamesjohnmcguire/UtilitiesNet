/////////////////////////////////////////////////////////////////////////////
// $Id$
// <copyright file="OrderedItem.cs" company="James John McGuire">
// Copyright © 2006 - 2019 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using NUnit.Framework;
using System.IO;

namespace DigitalZenWorks.Common.Utils
{
	public class OrderedItem
	{
		public string ItemName { get; set; }
		public string Description { get; set; }
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		public decimal LineTotal { get; set; }
	}
}
