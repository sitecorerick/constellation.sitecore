namespace Constellation.Sitecore.Validators
{
	using System;
	using System.Runtime.Serialization;

	using global::Sitecore.Data.Validators;

	using global::Sitecore.Diagnostics;

	/// <summary>
	/// Defines the Specific Number Selected Validator
	/// </summary>
	[Serializable]
	public class SpecificNumberSelectedValidator : StandardValidator
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SpecificNumberSelectedValidator"/> class. 
		/// </summary>
		public SpecificNumberSelectedValidator()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SpecificNumberSelectedValidator"/> class. 
		/// </summary>
		/// <param name="info">
		/// The info
		/// </param>
		/// <param name="context">
		/// The context
		/// </param>
		public SpecificNumberSelectedValidator(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// Gets the name of the validator, SpecificNumberSelectedValidator
		/// </summary>
		public override string Name
		{
			get { return "SpecificNumberSelectedValidator"; }
		}

		/// <summary>
		/// Evaluates the current item for number of selected values
		/// </summary>
		/// <returns>The result of the evaluation</returns>
		protected override ValidatorResult Evaluate()
		{
			var controlValidationValue = this.ControlValidationValue;
			var numberallowed = this.Parameters["numberallowed"];
			Assert.IsNotNullOrEmpty(numberallowed, "numberallowed parameter must be supplied for SpecificNumberSelectedValidator.");
			var num = int.Parse(numberallowed);
			if (!string.IsNullOrEmpty(controlValidationValue))
			{
				var selected = controlValidationValue.Split(new[] { '|' });
				var count = selected.Length;
				if (count <= num)
				{
					return ValidatorResult.Valid;
				}

				this.Text = this.GetText("Too many items selected! The field \"{0}\" only allows {1} item(s) to be selected", this.GetFieldDisplayName(), numberallowed);
				return this.GetFailedResult(ValidatorResult.CriticalError);
			}

			return ValidatorResult.Valid;
		}

		/// <summary>
		/// Gets the max validator result
		/// </summary>
		/// <returns>The max validator result</returns>
		protected override ValidatorResult GetMaxValidatorResult()
		{
			return this.GetFailedResult(ValidatorResult.CriticalError);
		}
	}
}
