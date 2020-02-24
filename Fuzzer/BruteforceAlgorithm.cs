using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuzzer
{
    public class BruteforceAlgorithm
    {
        public event Action<int> OnStringLengthChanged;
        public event Action<string> OnStringComputed;
        public event Action OnEnded;

        private char[] Characters
        {
            get;
            set;
        }
        private int CharactersLength
        {
            get;
            set;
        }

        private int MaxLength
        {
            get;
            set;
        }
        public BruteforceAlgorithm(int maxStringLength, char[] characters)
        {
            this.Characters = characters;
            this.CharactersLength = characters.Length;
            this.MaxLength = maxStringLength;
        }
        public void Start()
        {
            var estimatedPasswordLength = 0;

            for (int i = 0; i < MaxLength; i++)
            {
                estimatedPasswordLength++;
                OnStringLengthChanged(estimatedPasswordLength);
                startBruteForce(estimatedPasswordLength);
            }
            OnEnded?.Invoke();
        }
        private void startBruteForce(int keyLength)
        {
            var keyChars = createCharArray(keyLength, Characters[0]);
            var indexOfLastChar = keyLength - 1;
            createNewKey(0, keyChars, keyLength, indexOfLastChar);
        }
        private char[] createCharArray(int length, char defaultChar)
        {
            return (from c in new char[length] select defaultChar).ToArray();
        }

        private void createNewKey(int currentCharPosition, char[] keyChars, int keyLength, int indexOfLastChar)
        {
            var nextCharPosition = currentCharPosition + 1;

            for (int i = 0; i < CharactersLength; i++)
            {
                keyChars[currentCharPosition] = Characters[i];

                if (currentCharPosition < indexOfLastChar)
                {
                    createNewKey(nextCharPosition, keyChars, keyLength, indexOfLastChar);
                }
                else
                {
                    OnStringComputed(new String(keyChars));
                }
            }
        }
    }
}
