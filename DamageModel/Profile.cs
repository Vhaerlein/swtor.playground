using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TorPlayground.DamageModel
{
	public class Profile
	{
		public bool HasUnsavedChanges { get; set; }

		protected class ProfileInternal
		{
			public Configuration Configuration { get; set; }
			public List<Session> Sessions { get; set; }
			public Session DefaultValues { get; set; }
		}

		public Configuration Configuration => _profile.Configuration ?? (_profile.Configuration = new Configuration());
		public List<Session> Sessions => _profile.Sessions ?? (_profile.Sessions = new List<Session>());
		public Session DefaultValues => _profile.DefaultValues ?? (_profile.DefaultValues = new Session());

		public Session ActiveSession
		{
			get
			{
				return Sessions?.FirstOrDefault(s => s.Active) ?? Sessions?.FirstOrDefault();
			}
		}

		private readonly ProfileInternal _profile;

		private Profile(ProfileInternal profile)
		{
			_profile = profile;
		}

		internal static Profile Parse(string input)
		{
			var profile = JsonConvert.DeserializeObject<ProfileInternal>(input);
			return new Profile(profile);
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(_profile, Formatting.Indented);
		}

		public double Dps { get; private set; }

		public void UpdateDpsEstimation()
		{
			Dps = DpsUtils.CalculateDps(this);
		}

		public void UpdateDpsEstimationAllSessions()
		{
			Dps = Sessions.Select(session => DpsUtils.CalculateDps(session, Configuration)).Average();
		}
	}
}
