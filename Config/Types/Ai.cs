using System.Collections.Generic;

namespace AiDisabler.Config.Types {
    public class Ai {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ClassId { get; set; }
        public string TypeId { get; set; }
        public bool? IsDisabled { get; set; }
        public bool? IsKilled { get; set; }

        public override bool Equals(object obj) {
            if (obj == null || !(obj is Ai))
                return false;
            Ai objectTest = (Ai)obj;

            bool hasmatch = false;
            if (objectTest.ID != null && this.ID != null)
                if (objectTest.ID.ToLower() == this.ID.ToLower())
                    hasmatch = true;
            if (objectTest.Name != null && this.Name != null)
                if (objectTest.Name.ToLower() == this.Name.ToLower())
                    hasmatch = true;
            if (objectTest.ClassId != null && this.ClassId != null)
                if (objectTest.ClassId.ToLower() == this.ClassId.ToLower())
                    hasmatch = true;
            if (objectTest.TypeId != null && this.TypeId != null)
                if (objectTest.TypeId.ToLower() == this.TypeId.ToLower())
                    hasmatch = true;

            return hasmatch;
        }

        public override int GetHashCode() {
            return ID.ToLower().GetHashCode() ^ Name.ToLower().GetHashCode() ^ ClassId.ToLower().GetHashCode() ^ TypeId.ToLower().GetHashCode() ^ IsDisabled.GetHashCode() ^ IsKilled.GetHashCode();
        }
    }
}
