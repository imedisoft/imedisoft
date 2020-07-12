using CodeBase;
using DataConnectionBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace OpenDentBusiness
{
    /// <summary>
    /// Used in convert script and thus cannot change functionality without affecting conversion history.
    /// If a function needs to be changed drastically, then create necessary polymorphisms of the function to handle the new scenario.
    /// This class contains methods used to handle large tables - 
    /// The public variables ARE NOT thread safe - The functions within run things in thread, do not call functions in this class in threads. 
    /// USE SETTERS FOR THIS CLASS PRIOR TO RUNNING ANY LARGE TABLE COMMANDS!!!!!
    /// TableName and TablePriKeyField are REQUIRED.  
    /// ServerTo, DatabaseTo, UserTo, and PasswordTo are needed if you want to switch database connections prior to running commands.
    /// </summary>
    public class LargeTableHelper
	{
		public static string GetCurrentDatabase() => Db.GetScalar("SELECT database()");
	}
}
