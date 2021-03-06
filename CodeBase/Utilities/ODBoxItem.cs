namespace CodeBase
{
    /// <summary>
    /// A helper "item" for list boxes so that we do not have to override .ToString() on all of our objects.
    /// </summary>
    public class ODBoxItem<T>
	{
        /// <summary>
		/// Set text to the phrase that should display to the user.
		/// Set tag to the object that corresponds to the text.
		/// ODComboItem has overridden .ToString() to simply return the passed in text.
		/// This is so that we can easily get objects from combo boxes without having to have a synchronized local list of objects.
		/// </summary>
        public ODBoxItem(string text, T tag = default)
		{
			Text = text;
			Tag = tag;
		}

        public string Text { get; set; }

        public T Tag { get; set; }

        public override string ToString() => Text;
	}
}
