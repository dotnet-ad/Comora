using System;
using Android.Views;
using Android.Content;
using Facebook.Yoga;

namespace Flexml.Droid
{
    public class FlexLayout : Absolu
    {
        public FlexLayout(Context context, YogaNode node) : base(context)
        {
        }

        private YogaNode node;

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            
        }
    }
}
