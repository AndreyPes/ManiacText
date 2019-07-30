using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiacText
{
    class Element
    {
        public Element(string value)
        { Value = value; }

        public string Value { get; private set; }

        public bool IsInText { get; set; }
    }

    class Program
    {

        static void Main(string[] args)
        {
            string _maniacText = "Her name was Angela Merkel asd asd asd asd вввыы rtt said that she".ToLower();
            string[] _text = @"The case is among a number of high-profile crimes said to involve asylum seekers which have stoked anger against migrants and put pressure on Chancellor Angela Merkel over her liberal refugee policy.
Regular demonstrations have been held in Kandel, home to 9,000 people, by the far - right Alternative for Germany(AfD) in an attempt to bolster its anti - migrant campaign.
On the other side of the country - in Chemnitz - thousands of people have taken to the streets to protest, after a German - Cuban man was stabbed to death there on 26 August.
After the protests on Saturday, the victim's widow told German media: Daniel would never have wanted it! Never!
She said her husband was neither right - nor left - wing and the protests in his name were not about Daniel anymore.
Several of Germany's prominent bands are expected to attract thousands to Chemnitz on Monday evening for a free concert against the far-right and xenophobia, under the slogan #wirsindmehr.".ToLower().Split(' ');

            Console.WriteLine(ManiacMessage(_maniacText, _text));
            Console.ReadLine();

        }

        static string ManiacMessage(string maniacText, string[] text)
        {

            StringBuilder _sb = new StringBuilder();
            string[] _wordArr = maniacText.Split(' ');
            Element[] _wordDectionary = new Element[_wordArr.Count()];
            int _iter = 0;
            var sync = new object();
            foreach (var s in _wordArr)
            {
                _wordDectionary[_iter] = new Element(s);
                _iter++;
            }

            Parallel.ForEach(text, (s) =>
            {
                lock (sync)
                {
                    if (_wordDectionary.Where(x => x.Value == s.ToLower() && x.IsInText == false).Count() > 0)
                    {
                        _wordDectionary.FirstOrDefault(x => x.Value == s && x.IsInText == false).IsInText = true;
                    }
                }

            });

            foreach (var s in _wordDectionary)
            {
                if (s.IsInText)
                {
                    if (_sb.Length != 0)
                    {
                        _sb.Append(" ");
                        _sb.Append(s.Value);

                    }

                    if (_sb.Length == 0)
                        _sb.Append(s.Value);
                }
            }
            return _sb.ToString();
        }
    }
}
