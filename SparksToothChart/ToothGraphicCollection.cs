using System;
using System.Collections;

namespace SparksToothChart
{
    /// <summary>
    /// A strongly typed collection of type ToothGraphic
    /// </summary>
    public class ToothGraphicCollection : CollectionBase
	{
		/// <summary>
		/// Returns the ToothGraphic with the given index.
		/// </summary>
		public ToothGraphic this[int index]
		{
			get
			{
				return (ToothGraphic)List[index];
			}
			set
			{
				List[index] = value;
			}
		}

		/// <summary>
		/// Returns the ToothGraphic with the given toothID.
		/// </summary>
		public ToothGraphic this[string toothID]
		{
			get
			{
				if (toothID != "implant" && !ToothGraphic.IsValidToothID(toothID))
				{
					throw new ArgumentException("Tooth ID not valid: " + toothID);
				}

				for (int i = 0; i < List.Count; i++)
				{
					if (((ToothGraphic)List[i]).ToothID == toothID)
					{
						return (ToothGraphic)List[i];
					}
				}

				return null;
			}
		}

		public int Add(ToothGraphic value)
		{
			return List.Add(value);
		}

		public int IndexOf(ToothGraphic value)
		{
			return List.IndexOf(value);
		}

		public void Insert(int index, ToothGraphic value)
		{
			List.Insert(index, value);
		}

		public void Remove(ToothGraphic value)
		{
			List.Remove(value);
		}

		public bool Contains(ToothGraphic value)
		{
			return List.Contains(value);
		}

		protected override void OnInsert(int index, Object value)
		{
			if (value.GetType() != typeof(ToothGraphic))
			{
				throw new ArgumentException("value must be of type ToothGraphic.", "value");
			}
		}

		protected override void OnRemove(int index, Object value)
		{
			if (value.GetType() != typeof(ToothGraphic))
			{
				throw new ArgumentException("value must be of type ToothGraphic.", "value");
			}
		}

		protected override void OnSet(int index, Object oldValue, Object newValue)
		{
			if (newValue.GetType() != typeof(ToothGraphic))
			{
				throw new ArgumentException("newValue must be of type ToothGraphic.", "newValue");
			}
		}

		protected override void OnValidate(Object value)
		{
			if (value.GetType() != typeof(ToothGraphic))
			{
				throw new ArgumentException("value must be of type ToothGraphic.");
			}
		}

		public ToothGraphicCollection Copy()
		{
			var collect = new ToothGraphicCollection();

			for (int i = 0; i < Count; i++)
			{
				collect.Add(this[i].Copy());
			}

			return collect;
		}
	}
}
