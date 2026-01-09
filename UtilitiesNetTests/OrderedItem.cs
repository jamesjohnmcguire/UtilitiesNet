/////////////////////////////////////////////////////////////////////////////
// <copyright file="OrderedItem.cs" company="Digital Zen Works">
// Copyright © 2006 - 2026 Digital Zen Works.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.Common.Utilities.Tests;

#pragma warning disable IDE0079
#pragma warning disable CA1515
/// <summary>
/// Ordered item class.
/// </summary>
public class OrderedItem
{
	/// <summary>
	/// Gets or sets the item name.
	/// </summary>
	/// <value>The item name.</value>
	public string? ItemName { get; set; }

	/// <summary>
	/// Gets or sets the description.
	/// </summary>
	/// <value>The description.</value>
	public string? Description { get; set; }

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
#pragma warning restore CA1515
#pragma warning restore IDE0079
