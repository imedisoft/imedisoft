using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;

namespace OpenDentBusiness {
	/// <summary>An Xray sensor. Depending on the hardware, this can either be one physical sensor or a set of similar sensors.  It can also represent how one sensor is connected to a single workstation.</summary>
	[Serializable()]
	public class Sensor /*: TableBase*/ {
		/*
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long SensorNum;
		/// <summary>Any description of the sensor.</summary>
		public string Description;
		/// <summary>Name of the workstation for this sensor.  Optional.</summary>
		public string Workstation;
		*/


		/*
		///<summary>FK to patient.PatNum</summary>
		public long PatNum;
		///<summary>FK to definition.DefNum. Categories for documents.</summary>
		public long DocCategory;
		/// <summary>The date/time at which the mount itself was created. Has no bearing on the creation date of the images the mount houses.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateCreated;
		/// <summary>Used to provide a document description in the image module tree-view.</summary>
		public string Description;
		/// <summary>To allow the user to enter specific information regarding the exam and tooth numbers, as well as points of interest in the xray images.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string Note;
		/// <summary>The width of the mount, in pixels.</summary>
		public int Width;
		/// <summary>The height of the mount, in pixels.</summary>
		public int Height;
		*/


	}
}
