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

namespace SinglePageApplicationWPF
{
    using SinglePageApplicationWPF.Base;

    public class RowItemPosition : EnumBase
    {
        public static readonly RowItemPosition None = new NonePosition();
        public static readonly RowItemPosition GoFirst = new GoFirstPosition();
        public static readonly RowItemPosition GoLast = new GoLastPosition();
        public static readonly RowItemPosition GoMove = new GoMovePosition();

        private int movePos = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="RowItemPosition"/> class.
        /// </summary>
        private RowItemPosition(int value, string name = null, string description = null) : base(value, name, description)
        {
        }


        public int GoTo
        {
            get { return movePos; }
            set { movePos = value; }
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
    }
}
