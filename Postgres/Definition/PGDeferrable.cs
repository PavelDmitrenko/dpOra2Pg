using System;

namespace dpOra2Pg
{
	public class PGDeferrable
	{
        public string Deferrable { get; }
		public string Deferred { get; }

		public PGDeferrable(string deferrable, string deferred)
        {
            if (!deferrable.Equals("NOT DEFERRABLE", StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentOutOfRangeException($"Unknown deferrable type ({deferrable})");

			if (!deferred.Equals("IMMEDIATE", StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentOutOfRangeException($"Unknown deferred type ({deferred})");

			Deferrable = deferrable.ToUpper();
            Deferred = deferred.ToUpper();
        }

    }
	
}
