using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
public class NetworkPlayerController : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer characterRenderer;
    [SerializeField] List<Material> characterMaterials;
    [SerializeField] Transform hats;
    
    public void UpdateSkin(int skinId){
        this.GetComponent<PhotonView>().RPC("SetMaterial",RpcTarget.All, skinId, this.GetComponent<PhotonView>().Owner);
    }

    /// <summary>
    /// Atualiza o chapeu do jogador, se for valor menor que 0 não deixa nenhum chapeu ligado
    /// </summary>
    /// <param name="hatId"></param>
    public void UpdateHat(int hatId){
        this.GetComponent<PhotonView>().RPC("SetHat",RpcTarget.All, hatId, this.GetComponent<PhotonView>().Owner);
    }

    [PunRPC]
    public void SetMaterial(int materialId, Player owner){
        if(this.GetComponent<PhotonView>().Owner == owner){
            characterRenderer.material = characterMaterials[materialId];
        }
    }

    [PunRPC]
    public void SetHat(int hatId, Player owner){
        if(this.GetComponent<PhotonView>().Owner == owner){
            foreach(Transform t in hats){
                t.gameObject.SetActive(false);
            }


            if(hatId >= 0)
                hats.GetChild(hatId).gameObject.SetActive(true);
        }
    }
}
