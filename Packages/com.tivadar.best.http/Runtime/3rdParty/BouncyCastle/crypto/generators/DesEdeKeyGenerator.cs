#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

using Best.HTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace Best.HTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
    public class DesEdeKeyGenerator
		: DesKeyGenerator
    {
		public DesEdeKeyGenerator()
		{
		}

		internal DesEdeKeyGenerator(
			int defaultStrength)
			: base(defaultStrength)
		{
		}

		/**
        * initialise the key generator - if strength is set to zero
        * the key Generated will be 192 bits in size, otherwise
        * strength can be 128 or 192 (or 112 or 168 if you don't count
        * parity bits), depending on whether you wish to do 2-key or 3-key
        * triple DES.
        *
        * @param param the parameters to be used for key generation
        */
        protected override void EngineInit(KeyGenerationParameters parameters)
        {
			this.random = parameters.Random;
			this.strength = (parameters.Strength + 7) / 8;

			if (strength == 0 || strength == (168 / 8))
            {
                strength = DesEdeParameters.DesEdeKeyLength;
            }
            else if (strength == (112 / 8))
            {
                strength = 2 * DesEdeParameters.DesKeyLength;
            }
            else if (strength != DesEdeParameters.DesEdeKeyLength
                && strength != (2 * DesEdeParameters.DesKeyLength))
            {
                throw new ArgumentException("DESede key must be "
                    + (DesEdeParameters.DesEdeKeyLength * 8) + " or "
                    + (2 * 8 * DesEdeParameters.DesKeyLength)
                    + " bits long.");
            }
        }

        protected override byte[] EngineGenerateKey()
        {
            byte[] newKey = new byte[strength];

            do
            {
                random.NextBytes(newKey);
                DesEdeParameters.SetOddParity(newKey);
            }
            while (DesEdeParameters.IsWeakKey(newKey, 0, newKey.Length) || !DesEdeParameters.IsRealEdeKey(newKey, 0));

            return newKey;
        }
    }
}
#pragma warning restore
#endif
