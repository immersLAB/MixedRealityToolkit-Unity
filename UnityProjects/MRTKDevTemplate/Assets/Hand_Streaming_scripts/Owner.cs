namespace Photon.Pun
{
    using UnityEngine;

    public class Owner : MonoBehaviour
    {
        private PhotonView script;
        public bool owner = false;

        // Start is called before the first frame update
        void Start()
        {
            PhotonView script = this.GetComponent<PhotonView>();
            script.IsMine = owner;
        }

        // Update is called once per frame
        void Update()
        {
            PhotonView script = this.GetComponent<PhotonView>();
            script.IsMine = owner;
           
        }
    }
}
