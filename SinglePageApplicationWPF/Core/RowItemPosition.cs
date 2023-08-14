//-----------------------------------------------------------------------
// <copyright file="RowItemPosition.cs" company="www.pta.de">
//     Class: RowItemPosition
//     Copyright © www.pta.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - www.pta.de</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>11.08.2023 14:45:40</date>
//
// <summary>
// Enum Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace SinglePageApplicationWPF.Core
{
    using SinglePageApplicationWPF.Base;

    public class RowItemPosition : EnumBase
    {
        public static readonly RowItemPosition None = new NonePosition();
        public static readonly RowItemPosition GoFirst = new GoFirstPosition();
        public static readonly RowItemPosition GoLast = new GoLastPosition();
        public static readonly RowItemPosition GoMove = new GoMovePosition();
        public static readonly RowItemPosition GoNew = new GoNewPosition();
        public static readonly RowItemPosition GoBeforeDelete = new GoBeforeDeletePosition();
        public static readonly RowItemPosition GoAfterDelete = new GoAfterDeletePosition();

        private int movePos = 0;
        private Guid entityId = Guid.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="RowItemPosition"/> class.
        /// </summary>
        private RowItemPosition(int value, string name = null, string description = null) : base(value, name, description)
        {
        }


        public int GoTo
        {
            get { return this.movePos; }
            set { this.movePos = value; }
        }

        public Guid EntityId
        {
            get { return this.entityId; }
            set { this.entityId = value; }
        }


        private class NonePosition : RowItemPosition
        {
            public NonePosition() : base(0, "None")
            {
            }
        }

        private class GoFirstPosition : RowItemPosition
        {
            public GoFirstPosition() : base(1, "GoFirst")
            {
            }
        }

        private class GoLastPosition : RowItemPosition
        {
            public GoLastPosition() : base(2, "GoLast")
            {
            }
        }

        private class GoMovePosition : RowItemPosition
        {
            public GoMovePosition() : base(3, "GoMove")
            {
            }
        }

        private class GoNewPosition : RowItemPosition
        {
            public GoNewPosition() : base(4, "GoNew")
            {
            }
        }

        private class GoBeforeDeletePosition : RowItemPosition
        {
            public GoBeforeDeletePosition() : base(5, "GoBeforeDelete")
            {
            }
        }

        private class GoAfterDeletePosition : RowItemPosition
        {
            public GoAfterDeletePosition() : base(6, "GoAfterDelete")
            {
            }
        }
    }
}
