using System;

namespace Imedisoft.UI
{
    /// <summary>
    /// Primarily used to attach help subjects to dialog forms. Forms with a help subject attached 
    /// will display a help icon in the titlebar.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class HelpSubjectAttribute : Attribute
    {
        /// <summary>
        /// Gets the help subject.
        /// </summary>
        public string Subject { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpSubjectAttribute"/> class.
        /// </summary>
        public HelpSubjectAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpSubjectAttribute"/> class with the given subject.
        /// </summary>
        /// <param name="subject">The help subject.</param>
        public HelpSubjectAttribute(string subject)
        {
            Subject = subject;
        }
    }
}
