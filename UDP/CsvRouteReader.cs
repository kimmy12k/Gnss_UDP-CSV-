using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace UDPMode
{
    internal class CsvRouteReader
    {
        public struct RoutePoint
        {
            public double Time;
            public double Latitude;
            public double Longitude;
            public double Altitude;
        }

        private readonly List<RoutePoint> _points = new List<RoutePoint>();
        private int _currentIndex = 0;

        public int Count => _points.Count;
        public int CurrentIndex => _currentIndex;
        public bool IsFinished => _currentIndex >= _points.Count;
        public string FilePath { get; private set; }

        public void Load(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"경로 파일을 찾을 수 없습니다: {filePath}");
            FilePath = filePath;
            _points.Clear();
            _currentIndex = 0;
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;
                string[] parts = line.Split(',');
                if (parts.Length < 4) continue;
                try
                {
                    var point = new RoutePoint
                    {
                        Time = double.Parse(parts[0].Trim(), CultureInfo.InvariantCulture),
                        Latitude = double.Parse(parts[1].Trim(), CultureInfo.InvariantCulture),
                        Longitude = double.Parse(parts[2].Trim(), CultureInfo.InvariantCulture),
                        Altitude = double.Parse(parts[3].Trim(), CultureInfo.InvariantCulture),
                    };
                    _points.Add(point);
                }
                catch { }
            }
            if (_points.Count == 0)
                throw new InvalidDataException("유효한 경로 데이터가 없습니다.");
        }

        public RoutePoint GetNext()
        {
            if (_points.Count == 0) throw new InvalidOperationException("경로가 로드되지 않았습니다.");
            if (_currentIndex < _points.Count) return _points[_currentIndex++];
            return _points[_points.Count - 1];
        }

        public RoutePoint GetAt(int index)
        {
            if (index < 0 || index >= _points.Count)
                throw new IndexOutOfRangeException($"인덱스 범위 초과: {index} (총 {_points.Count}개)");
            return _points[index];
        }

        public void Reset() { _currentIndex = 0; }
        public void ResetIndex() { _currentIndex = 0; }

        public double TotalDuration
        {
            get
            {
                if (_points.Count < 2) return 0;
                return _points[_points.Count - 1].Time - _points[0].Time;
            }
        }
    }
}
