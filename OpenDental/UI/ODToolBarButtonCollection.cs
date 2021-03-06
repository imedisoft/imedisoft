using System.Collections;

namespace OpenDental.UI
{
    public class ODToolBarButtonCollection : CollectionBase
	{

		/// <summary>
		/// Returns the Button with the given index.
		/// </summary>
		public ODToolBarButton this[int index]
		{
			get
			{
				return ((ODToolBarButton)List[index]);
			}
			set
			{
				List[index] = value;
			}
		}

		/// <summary>
		/// Returns the Button with the given string tag.
		/// </summary>
		public ODToolBarButton this[string buttonTag]
		{
			get
			{
				foreach (ODToolBarButton button in List)
                {
					if (button.Tag?.ToString() == buttonTag)
                    {
						return button;
                    }
                }

				return null;
			}
		}

		public void Add(ODToolBarButton button)
		{
			List.Add(button);
		}

		public void Remove(int index)
		{
			if ((index > Count - 1) || (index < 0))
			{
				throw new System.IndexOutOfRangeException();
			}
			else
			{
				List.RemoveAt(index);
			}
		}

		public int IndexOf(ODToolBarButton value)
		{
			return (List.IndexOf(value));
		}

		/// <summary>
		/// Returns the index of the button for the given tag. Returns -1 if a no button is found that matches the tag.
		/// </summary>
		public int IndexOf(object buttonTag)
		{
			for (int i = 0; i < List.Count; i++)
			{
				if (((ODToolBarButton)List[i]).Tag == buttonTag)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
