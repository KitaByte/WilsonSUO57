using System;

namespace Server.Custom.Misc
{
    public class WebStone : Item
    {
        private const string url = @"https://www.uoopenai.com/"; // Set to your url

        [Constructable]
        public WebStone() : base(0xED4)
        {
            Name = "Web Stone";

            Movable = false;

            Hue = 2734; // Gold Hue
        }

        public WebStone(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            try
            {
                from.LaunchBrowser(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening URL: {ex.Message}");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}
