﻿namespace TorPlayground.LogParser.Log
{
	public class Amount
	{
		public int Value { get; internal set; }
		public AmountType Type { get; internal set; }
		public AmountModifier Modifier { get; internal set; }
	}
}
