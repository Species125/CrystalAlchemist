

namespace CrystalAlchemist
{
    public static class GameUtil
    {
        public static void SetPreset(CharacterPreset source, CharacterPreset target)
        {
            target.setRace(source.getRace());
            target.AddColorGroupRange(source.GetColorGroupRange());
            target.AddProperty(source.GetProperties());
        }

        public static float setResource(float resource, float max, float addResource)
        {
            if (addResource != 0)
            {
                if (resource + addResource > max) addResource = max - resource;
                else if (resource + addResource < 0) resource = 0;

                resource += addResource;
            }

            return resource;
        }
    }
}

