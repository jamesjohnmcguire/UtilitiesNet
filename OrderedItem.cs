/////////////////////////////////////////////////////////////////////////////
// $Id$
// <copyright file="OrderedItem.cs" company="James John McGuire">
// Copyright © 2006 - 2019 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.Common.Utilities
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
