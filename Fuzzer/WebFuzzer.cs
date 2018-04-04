using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fuzzer
{
    public enum WebFuzzerError
    {
        Unknown = -1,
        InvalidUrl = 0,
        OfflineHost = 1,

    }
    public class WebFuzzer : IDisposable
    {
        private static char[] URL_CHARACTERS =
        {
           'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
           'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
           'u', 'v', 'w', 'x', 'y', 'z','A','B','C','D','E',
           'F','G','H','I','J','K','L','M','N','O','P','Q','R',
           'S','T','U','V','W','X','Y','Z','1','2','3','4','5',
           '6','7','8','9','0','_','-'
        };

        public event Action OnEnded;
        public event Action<WebFuzzerError> OnError;
        public event Action<string> OnUrlExist;
        public event Action<int> OnBruteforceStringLengthChanged;
        public event Action<string> OnUrlComputed;

        private Client Client
        {
            get;
            set;
        }
        private string Url
        {
            get;
            set;
        }

        private string Extension
        {
            get;
            set;
        }

        public WebFuzzer(string url, string extension = "")
        {
            Url = url;
            Extension = extension;
            Client = new Client();
            Client.HeadOnly = true;
        }
        public void StartAsync(int maxCharacterLength)
        {
            new Thread(new ThreadStart(new Action(() => Start(maxCharacterLength)))).Start();
        }
        public void Start(int maxCharacterLength)
        {
            if (Url[Url.Length - 1] != '/')
            {
                Url += "/";
            }
            if (!IsUrlValid(Url))
            {
                OnError?.Invoke(WebFuzzerError.InvalidUrl);
                return;
            }
            if (Client.GetHttpResponseCode(Url).HasValue == false)
            {
                OnError?.Invoke(WebFuzzerError.OfflineHost);
                return;
            }

            BruteforceAlgorithm bruteforce = new BruteforceAlgorithm(maxCharacterLength, URL_CHARACTERS);
            bruteforce.OnStringComputed += OnUrlReadyToCompute;
            bruteforce.OnStringLengthChanged += OnStringLengthChanged;
            bruteforce.OnEnded += Bruteforce_OnEnded;
            bruteforce.Start();
        }

        private void Bruteforce_OnEnded()
        {
            OnEnded?.Invoke();
        }

        private void OnStringLengthChanged(int obj)
        {
            OnBruteforceStringLengthChanged?.Invoke(obj);
        }

        private bool IsUrlValid(string url)
        {
            Uri uriResult;
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                  && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
        private void OnUrlReadyToCompute(string obj)
        {
            Client.Dispose();
            Client = new Client();
            Client.HeadOnly = true;

            string url = Url + obj + Extension;

            HttpStatusCode? statusCode = Client.GetHttpResponseCode(url);
            bool exist = statusCode == HttpStatusCode.OK;

            OnUrlComputed?.Invoke(url);
            if (exist)
            {
                OnUrlExist?.Invoke(url);
            }
        }

        public void Dispose()
        {
            Client.Dispose();
            Client = null;
            Url = null;
            Extension = null;
            OnEnded = null;
            OnError = null;
            OnUrlExist = null;
            OnBruteforceStringLengthChanged = null;
        }
    }
}
