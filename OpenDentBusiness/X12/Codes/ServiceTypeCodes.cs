using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imedisoft.X12.Codes
{
    /// <summary>
    /// These codes identify business groupings for health care services or benefits.
    /// </summary>
    /// <seealso href="https://x12.org/codes/service-type-codes"/>
    public static class ServiceTypeCodes
	{
		public const string None = "";

		/// <summary>
		/// Diagnostic x-ray provided by a healthcare provider.
		/// </summary>
		public const string DiagnosticXRay = "4";

		/// <summary>
		/// The translation of data gathered by clinical and radiographic examination into an 
		/// organized, classified definition of conditions present.
		/// </summary>
		public const string DiagnosticDental = "23";

		/// <summary>
		/// The art and science of examination, diagnosis, and treatment of diseases affecting the 
		/// periodontium; a study of the supporting structures of the teeth, normal anatomy and 
		/// physiology and the deviations.
		/// </summary>
		public const string Periodontics = "24";

		/// <summary>
		/// Broad term applied to any restorations to the tooth/teeth structure(s). Anterior teeth
		/// include up to five surface classifications - Mesial, Distal, Incisal, Lingual and 
		/// Labial. Posterior teeth include up to five surface classifications: Mesial, Distal, 
		/// Occlusal, Lingual and Buccal.
		/// </summary>
		public const string Restorative = "25";

		/// <summary>
		/// The branch of dentistry that is concerned with the morphology, physiology and pathology
		/// of the dental pulp and periradicular (gum) tissues.
		/// </summary>
		public const string Endodontics = "26";

		/// <summary>
		/// The branch of prosthetics is concerned with the restoration of stomatognathic and 
		/// associated facial structure that have been affected by disease, injury, surgery, or 
		/// congenital defect.
		/// </summary>
		public const string MaxillofacialProsthetics = "27";

		/// <summary>
		/// Typically these services involve a drug such as anesthesia or other substances that 
		/// serve as a supplemental purpose in dental therapy.
		/// </summary>
		public const string AdjunctiveDentalServices = "28";

		/// <summary>
		/// Plan coverage and general benefits for the member's policy or contract.
		/// </summary>
		public const string HealthBenefitPlanCoverage = "30";

		/// <summary>
		/// The treatment of the teeth and their supporting structures.
		/// </summary>
		public const string DentalCare = "35";

		/// <summary>
		/// An artificial replacement for the natural crown of the tooth covering all five surfaces
		/// (Anterior teeth surface classifications - Mesial, Distal, Incisal, Lingual and Labial. 
		/// Posterior teeth surface classifications: Mesial, Distal, Occlusal, Lingual and Buccal.
		/// </summary>
		public const string DentalCrowns = "36";

		/// <summary>
		/// Supplies or appliances for care of teeth due to accidental injury provided by 
		/// healthcare provider.
		/// </summary>
		public const string DentalAccident = "37";

		/// <summary>
		/// The area of dentistry concerned with the supervision, guidance, and correction of the 
		/// growing and mature orofacial structures. This includes conditions that require movement
		/// of the teeth or correction of the malrelationships and malformations of related 
		/// structures by the adjustment of relationships between and among teeth and facial bones 
		/// by the application of forces or the stimulation and redirection of functional forces 
		/// within the craniofacial complex.
		/// </summary>
		public const string Orthodontics = "38";

		/// <summary>
		/// The part of dentistry pertaining to the restoration and maintenance of oral function, 
		/// comfort, appearance and health of the patient by replacement of missing teeth and 
		/// contiguous tissues with artificial substitutes. It has three main branches: removable 
		/// prosthodontics, fixed prosthodontics and maxillofacial prosthetics.
		/// </summary>
		public const string Prosthodontics = "39";

		/// <summary>
		/// Diagnosis and treatment of disorders of the mouth, teeth, jaws and facial structure 
		/// provided by a healthcare provider
		/// </summary>
		public const string OralSurgery = "40";

		/// <summary>
		/// The dental procedures in dental practice and health programs that prevent the 
		/// occurrence of oral diseases.
		/// </summary>
		public const string PreventiveDental = "41";
	}
}
