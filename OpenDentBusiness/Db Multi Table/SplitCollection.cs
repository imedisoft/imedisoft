using CodeBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OpenDentBusiness
{
	public class SplitCollection : ICollection<PaySplit>
	{
		private readonly Dictionary<string, PaySplit> paySplits = new Dictionary<string, PaySplit>();

		public int Count 
			=> paySplits.Count;

		public bool IsReadOnly 
			=> false;

		private string GetUniqueKeyFromPaySplit(PaySplit paySplit)
		{
			if (paySplit.SplitNum > 0)
			{
				return paySplit.SplitNum.ToString();
			}

			if (paySplit.TagOD is string && ((string)paySplit.TagOD) != "")
			{
				return (string)paySplit.TagOD;
			}

			throw new ODException("Invalid PaySplit with no SplitNum or invalid TagOD");
		}

		public void Add(PaySplit paySplit)
		{
			string uniqueKey = GetUniqueKeyFromPaySplit(paySplit);

			if (paySplits.ContainsKey(uniqueKey))
			{
				return;
			}

			paySplits.Add(uniqueKey, paySplit);
		}

		public void Clear() 
			=> paySplits.Clear();

		public bool Contains(PaySplit paySplit) 
			=> paySplits.ContainsKey(GetUniqueKeyFromPaySplit(paySplit));

		public void CopyTo(PaySplit[] array, int arrayIndex)
		{
			for (int i = arrayIndex; i < paySplits.Values.Count; i++)
			{
				array[i] = paySplits.Values.ToList()[i];
			}
		}

		public bool Remove(PaySplit paySplit) 
			=> paySplits.Remove(GetUniqueKeyFromPaySplit(paySplit));

		public IEnumerator<PaySplit> GetEnumerator() 
			=> paySplits.Values.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() 
			=> paySplits.Values.GetEnumerator();
	}
}
