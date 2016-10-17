using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DtmfPad
{
    public class AppModel
    {
        public Collection<DtmfSignal> Signals { get; }

        public AppModel()
        {
            Signals = new Collection<DtmfSignal>
            {
                new DtmfSignal("1"),
                new DtmfSignal("2"),
                new DtmfSignal("3"),
                new DtmfSignal("4"),
                new DtmfSignal("5"),
                new DtmfSignal("6"),
                new DtmfSignal("7"),
                new DtmfSignal("8"),
                new DtmfSignal("9"),
                new DtmfSignal("*") { FileId = "star" },
                new DtmfSignal("0"),
                new DtmfSignal("#") { FileId = "pound" },
            };
        }
    }

    public class DtmfSignal
    {
        public string Id { get; }

        string fileId;
        public string FileId
        {
            get { return fileId ?? Id; }
            set { fileId = value; }
        }

        public string FileUri => $"Sounds/DTMF-{FileId}.mp3";

        public DtmfSignal(string id)
        {
            Id = id;
        }
    }
}
