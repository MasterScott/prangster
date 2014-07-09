﻿/*
 Copyright (c) 2013 Cylance, Inc.  All rights reserved.
 For updates, please visit <http://www.cylance.com/>.

 Redistribution and use in source and binary forms, with or without
 modification, are permitted provided that the following conditions are met:
 1. Redistributions of source code must retain the above copyright
    notice, this list of conditions and the following disclaimer.
 2. Redistributions in binary form must reproduce the above copyright
    notice, this list of conditions and the following disclaimer in the
    documentation and/or other materials provided with the distribution.
 3. All advertising materials mentioning features or use of this software
    must display the following acknowledgement:
    This product includes software developed by Cylance, Inc.
 4. Neither the name of Cylance, Inc., nor the names of its contributors
    may be used to endorse or promote products derived from this software
    without specific prior written permission.

 THIS SOFTWARE IS PROVIDED BY CYLANCE, INC., ''AS IS'' AND ANY EXPRESS OR
 IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO
 EVENT SHALL CYLANCE, INC., BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS;
 OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
 WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR
 OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;

namespace Cylance.Research.Prangster
{

    /// <summary>
    /// Base class from which pseudorandom number generator (PRNG) implementations are derived.
    /// </summary>
    public abstract class PrngBase
    {

        /// <summary>
        /// Represents the lowest 64-bit seed value that this PRNG will accept during initialization.
        /// </summary>
        public abstract ulong MinimumSeed
        {
            get;
        }

        /// <summary>
        /// Represents the largest 64-bit seed value that this PRNG will accept during initialization.
        /// </summary>
        public abstract ulong MaximumSeed
        {
            get;
        }

        /// <summary>
        /// When overridden in a derived class, initializes the internal state of the PRNG using the supplied seed.
        /// </summary>
        /// <param name="seed">An integer between <see cref="MinimumSeed"/> and <see cref="MaximumSeed"/> with which to seed the PRNG.</param>
        public abstract void Seed(ulong seed);

        /// <summary>
        /// When overridden in a derived class, generates a pseudorandom integer and advances the internal state of the PRNG.
        /// </summary>
        /// <returns>The next pseudorandom integer generated by the PRNG.</returns>
        public abstract ulong Next();

        /// <summary>
        /// Generates a pseudorandom integer in the range [0..<paramref name="limit"/>) and advances the internal state of the PRNG.
        /// </summary>
        /// <param name="limit">The lowest positive integer greater than the maximum desired value.</param>
        /// <returns>The next pseudorandom integer generated by the PRNG modulo <paramref name="limit"/>.</returns>
        /// <remarks>
        /// This method should be used whenever appropriate instead of <see cref="Next()"/>, as some
        /// PRNGs offer a specialized implementation for generating a pseudorandom number within a range.
        /// </remarks>
        public virtual ulong Next(ulong limit)
        {
            return (this.Next() % limit);
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the PRNG supports reversing.
        /// </summary>
        public virtual bool CanReverse
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// When overridden in a derived class, generates a pseudorandom integer and reverses the internal state of the PRNG, if the PRNG supports reversing.
        /// </summary>
        /// <returns>The previous pseudorandom integer generated by the PRNG.</returns>
        /// <exception cref="System.NotImplementedException">The PRNG does not support reversing.</exception>
        public virtual ulong Previous()
        {
            throw new NotImplementedException("This PRNG does not support reversing.");
        }

        /// <summary>
        /// Generates a pseudorandom integer in the range [0..<paramref name="limit"/>) and reverses the internal state of the PRNG, if the PRNG supports reversing.
        /// </summary>
        /// <param name="limit">The lowest positive integer greater than the maximum desired value.</param>
        /// <returns>The previous pseudorandom integer generated by the PRNG modulo <paramref name="limit"/>.</returns>
        /// <exception cref="System.NotImplementedException">The PRNG does not support reversing.</exception>
        public virtual ulong Previous(ulong limit)
        {
            return (this.Previous() % limit);
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the PRNG supports seeking.
        /// </summary>
        public virtual bool CanSeek
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// When overridden in a derived class, advances the PRNG by <paramref name="offset"/> states.
        /// </summary>
        /// <param name="offset">The number of states to skip, starting with the current state.</param>
        public virtual void SeekAhead(ulong offset)
        {
            throw new NotImplementedException("This PRNG does not support seeking.");
        }

        /// <summary>
        /// When overridden in a derived class, reverses the PRNG by <paramref name="offset"/> states.
        /// </summary>
        /// <param name="offset">The number of states to skip, starting with the current state.</param>
        public virtual void SeekBack(ulong offset)
        {
            throw new NotImplementedException("This PRNG does not support seeking.");
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the PRNG supports seeking from one seed to another.
        /// </summary>
        public virtual bool CanSeekSeed
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// When overridden in a derived class, computes the seed representing the <paramref name="offset"/>'th state after the state corresponding to <paramref name="seed"/>.
        /// </summary>
        /// <param name="seed">The seed used to the determine the initial state relative to which to seek.</param>
        /// <param name="offset">The number of states to skip.</param>
        public virtual ulong SeekSeedAhead(ulong seed, ulong offset)
        {
            throw new NotImplementedException("This PRNG does not support seed seeking.");
        }

        /// <summary>
        /// When overridden in a derived class, computes the seed representing the <paramref name="offset"/>'th state preceding the state corresponding to <paramref name="seed"/>.
        /// </summary>
        /// <param name="seed">The seed used to the determine the initial state relative to which to seek.</param>
        /// <param name="offset">The number of states to skip.</param>
        public virtual ulong SeekSeedBack(ulong seed, ulong offset)
        {
            throw new NotImplementedException("This PRNG does not support seed seeking.");
        }

        /// <summary>
        /// Discovers the seed or seeds that produce the supplied PRNG output sequence.
        /// </summary>
        /// <param name="output">A sequence containing one or more PRNG output values (modulo <paramref name="limit"/>)
        /// and any number of instances of <paramref name="wildcard"/>.</param>
        /// <param name="limit">The lowest positive integer greater than the maximum allowed output value, or 0 if no limit was imposed on the output.</param>
        /// <param name="wildcard">A number which will match any PRNG output value when encountered in <paramref name="output"/>.</param>
        /// <param name="callback">A method to invoke with seed discovery or progress notifications.</param>
        /// <param name="progressInterval">The number of seeds to test between progress notifications, or 0 to not receive progress notifications.</param>
        /// <returns>true if seed discovery completed (even if no seed was discovered), or false if the callback canceled discovery or an error occurred.</returns>
        /// <remarks>If progress notifications are requested, <paramref name="callback"/> will be invoked for a final progress notification event before true is returned.</remarks>
        public virtual bool RecoverSeed(ulong[] output, ulong limit, ulong wildcard, RecoverSeedCallback callback, ulong progressInterval)
        {
            ulong seed = this.MinimumSeed;
            ulong maxseed = this.MaximumSeed;

            if (seed > maxseed)
                return false;

            return
                RecoverSeed(
                    seed, maxseed, 1,
                    output, limit, wildcard,
                    callback, progressInterval);
        }

        /// <summary>
        /// Discovers the seed or seeds that produce the supplied PRNG output sequence.
        /// </summary>
        /// <param name="seedStart">The first seed value to test.</param>
        /// <param name="seedEnd">The last seed value that could be tested.</param>
        /// <param name="seedIncrement">A positive integer specifying the interval at which to test seeds.
        /// A value of 1 tests each seed serially, a value of 2 tests every other seed, and so on.</param>
        /// <param name="output">A sequence containing one or more PRNG output values (modulo <paramref name="limit"/>)
        /// and any number of instances of <paramref name="wildcard"/>.</param>
        /// <param name="limit">The lowest positive integer greater than the maximum allowed output value, or 0 if no limit was imposed on the output.</param>
        /// <param name="wildcard">A number which will match any PRNG output value when encountered in <paramref name="output"/>.</param>
        /// <param name="callback">A method to invoke with seed discovery or progress notifications.</param>
        /// <param name="progressInterval">The number of seeds to test between progress notifications, or 0 to not receive progress notifications.</param>
        /// <returns>true if seed discovery completed (even if no seed was discovered), or false if the callback canceled discovery or an error occurred.</returns>
        /// <remarks>If progress notifications are requested, <paramref name="callback"/> will be invoked for a final progress notification event before true is returned.</remarks>
        protected virtual bool RecoverSeed(ulong seedStart, ulong seedEnd, ulong seedIncrement, ulong[] output, ulong limit, ulong wildcard, RecoverSeedCallback callback, ulong progressInterval)
        {
            if (seedStart > seedEnd || seedIncrement == 0)
                return false;

            if (callback == null)
                progressInterval = 0;

            ulong totalcount = unchecked((seedEnd - seedStart) / seedIncrement + 1);  // count = 0 (2**64) if minimum seed = 0, maxseed = 2**64 - 1, and seedIncrement = 1
            ulong totalcountdown = totalcount;

            RecoverSeedEventArgs args = new RecoverSeedEventArgs();

            ulong seed = seedStart;

            do
            {
                ulong countdown = ((totalcountdown < progressInterval || progressInterval == 0) ? totalcountdown : progressInterval);
                totalcountdown = unchecked(totalcountdown - countdown);  // allow integer underflow so that 0 == 2**64

                do
                {
                    this.Seed(seed);

                    int i;

                    if (limit != 0)
                    {
                        for (i = 0; i < output.Length; i++)
                        {
                            if (this.Next(limit) != output[i] && output[i] != wildcard)
                                break;
                        }
                    }
                    else
                    {
                        for (i = 0; i < output.Length; i++)
                        {
                            if (this.Next() != output[i] && output[i] != wildcard)
                                break;
                        }
                    }

                    if (i == output.Length)  // invoke callback for seed discovery notification
                    {
                        args.EventType = RecoverSeedEventType.SeedDiscovered;
                        args.Seed = seed;
                        args.CurrentAttempts = (totalcount - totalcountdown - countdown);
                        args.TotalAttempts = totalcount;

                        callback(args);

                        if (args.Cancel)
                            return false;
                    }

                    seed += seedIncrement;
                }
                while (unchecked(--countdown) != 0);  // allow integer underflow here, so that 0 is equivalent to 2**64

                if (progressInterval != 0)  // invoke callback for progress notification
                {
                    args.EventType = RecoverSeedEventType.ProgressNotification;
                    args.Seed = seed;
                    args.CurrentAttempts = (totalcount - totalcountdown - countdown);
                    args.TotalAttempts = totalcount;

                    callback(args);

                    if (args.Cancel)
                        return false;
                }
            }
            while (totalcountdown != 0);

            return true;
        } //PrngBase.RecoverSeed(ulong,ulong,ulong,ulong[],ulong,ulong,RecoverSeedCallback,ulong)

    } //class PrngBase

    public delegate void RecoverSeedCallback(RecoverSeedEventArgs args);

    public class RecoverSeedEventArgs
    {
        public RecoverSeedEventType EventType;
        public bool Cancel;
        public ulong Seed;
        public ulong CurrentAttempts;
        public ulong TotalAttempts;
    }

    public enum RecoverSeedEventType
    {
        SeedDiscovered,
        ProgressNotification
    }

} //namespace Cylance.Research.Prangster
