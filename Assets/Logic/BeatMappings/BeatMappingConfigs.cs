﻿using System.Collections.Generic;

namespace BeatMappingConfigs {

    public class MappingContainer
    {
        public List<EventConfig> eventData = new List<EventConfig>();
        public List<NoteConfig> noteData = new List<NoteConfig>();
        public List<ObstacleConfig> obstacleData = new List<ObstacleConfig>();
        public List<BookmarkConfig> bookmarkData = new List<BookmarkConfig>();

        public void sortMappings()
        {
            eventData.Sort(delegate (EventConfig obj1, EventConfig obj2) { return obj1.time.CompareTo(obj2.time); });
            noteData.Sort(delegate (NoteConfig obj1, NoteConfig obj2) { return obj1.time.CompareTo(obj2.time); });
            obstacleData.Sort(delegate (ObstacleConfig obj1, ObstacleConfig obj2) { return obj1.time.CompareTo(obj2.time); });
            bookmarkData.Sort(delegate (BookmarkConfig obj1, BookmarkConfig obj2) { return obj1.time.CompareTo(obj2.time); });
        }
    }


    public class EventConfig
    {
        public float time;
        public int type;
        public int value;
    }


    public class NoteConfig {
        public const int CUT_DIRECTION_TOP = 0;
        public const int CUT_DIRECTION_RIGHT = 1;
        public const int CUT_DIRECTION_BOTTOM = 2;
        public const int CUT_DIRECTION_LEFT = 3;
        public const int LINE_INDEX_0 = 0;
        public const int LINE_INDEX_1 = 1;
        public const int LINE_INDEX_2 = 2;
        public const int LINE_INDEX_3 = 3;
        public const int LINE_LAYER_0 = 0;
        public const int LINE_LAYER_1 = 1;
        public const int LINE_LAYER_2 = 2;
        public const int LINE_LAYER_3 = 3;
        public const int NOTE_TYPE_LEFT = 0;
        public const int NOTE_TYPE_RIGHT = 1;

        public float time;
        public int lineIndex;
        public int lineLayer;
        public int type;
        public int cutDirection;
    }


    public class ObstacleConfig
    {
        public float time;
        public int lineIndex;
        public int type;
        public float duration;
        public float width;
    }


    public class BookmarkConfig
    {
        public float time;
        public string name;
    }

}
