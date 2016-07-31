﻿using System.Collections.Generic;

namespace ZangSiSee.Models
{
    public class Comic : BaseModel
    {
        public string Title { get; }
        public string Url { get; }
        public bool Concluded { get; }
        public override string Id => Title;

        public Comic(string title, string url, bool concluded)
        {
            Title = title;
            Url = url;
            Concluded = concluded;
        }

        public override bool Equals(object obj)
        {
            return new ComicComparer().Equals(this, obj as Comic);
        }

        public override int GetHashCode()
        {
            return new ComicComparer().GetHashCode(this);
        }
    }

    public class ComicComparer : IEqualityComparer<Comic>
    {
        public bool Equals(Comic x, Comic y)
        {
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Title == y.Title;
        }

        public int GetHashCode(Comic obj)
        {
            return obj.Id != null ? obj.Id.GetHashCode() : base.GetHashCode();
        }
    }
}
