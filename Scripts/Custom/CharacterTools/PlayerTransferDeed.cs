using Server.Misc;

namespace Server.Items
{
    public class PlayerTransferDeed : Item
    {
        private string character_Name;

        private string character_Account;

        [Constructable]
        public PlayerTransferDeed() : base(0x14F0)
        {
            Name = "Player Transfer Deed";

            Hue = 2500;

            LootType = LootType.Blessed;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsEmpty() || character_Account == from.Account.Username)
            {
                StoreCharacterInfo(from);
            }
            else
            {
                if (IsEmpty())
                {
                    from.SendMessage(33, "The transfer deed is empty.");

                    return;
                }

                if (PlayerTransferCore.ExecuteTransfer(from, character_Name, character_Account, from.Account.Username))
                {
                    from.SendMessage(63, $"{character_Name} transferred to your account.");

                    Delete();
                }
            }
        }

        public void StoreCharacterInfo(Mobile from)
        {
            character_Name = from.Name;

            character_Account = from.Account.Username;

            from.SendMessage(63, $"Character {character_Name} from account {character_Account} stored in the transfer deed.");

            from.SendMessage(53, "You can now give this deed to another player to transfer the character.");

            Hue = 2734;
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(character_Name) && string.IsNullOrEmpty(character_Account);
        }

        public PlayerTransferDeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1);

            writer.Write(character_Name);

            writer.Write(character_Account);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version > 0)
            {
                character_Name = reader.ReadString();

                character_Account = reader.ReadString();
            }
        }
    }
}

