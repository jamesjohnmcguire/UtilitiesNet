/////////////////////////////////////////////////////////////////////////////
// <copyright file="OrderedItem.cs" company="James John McGuire">
// Copyright © 2006 - 2024 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.Common.Utilities
{
	/// <summary>
	/// Ordered item class.
	/// </summary>
	public class OrderedItem
	{
		/// <summary>
		/// Gets or sets the item name.
		/// </summary>
		/// <value>The item name.</value>
		public string ItemName { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the unit price.
		/// </summary>
		/// <value>The unit price.</value>
		public decimal UnitPrice { get; set; }

		/// <summary>
		/// Gets or sets the quantity.
		/// </summary>
		/// <value>The quantity.</value>
		public int Quantity { get; set; }

		/// <summary>
		/// Gets or sets the line total.
		/// </summary>
		/// <value>The line total.</value>
		public decimal LineTotal { get; set; }
	}
}
