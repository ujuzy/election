using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Election
{
    public class MyView : RecyclerView.ViewHolder
    {
        public View mainView { get; set; }

        public TextView firstName { get; set; }
        public TextView secondName { get; set; }
        public TextView thirdName { get; set; }
        public TextView votes { get; set; }
        public TextView percent { get; set; }

        public ImageView photo { get; set; }
        public ImageView checkbox { get; set; }

        public MyView(View view) : base(view)
        {
            mainView = view;

        }
    }
}
