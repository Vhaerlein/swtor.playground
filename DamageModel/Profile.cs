using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TorPlayground.DamageModel
{
	public class Profile
	{
		protected class ProfileInternal
		{
			public Configuration Configuration { get; set; }
			public List<Session> Sessions { get; set; }
		}

		public Configuration Configuration => _profile.Configuration;
		public List<Session> Sessions => _profile.Sessions ?? (_profile.Sessions = new List<Session>());

		public Session ActiveSession
		{
			get
			{
				return Sessions?.FirstOrDefault(s => s.Active);
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

			if(profile.Configuration == null)
				return null;

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
	}
}
