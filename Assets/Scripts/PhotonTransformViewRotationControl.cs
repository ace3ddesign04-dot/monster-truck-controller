using UnityEngine;

public class PhotonTransformViewRotationControl
{
	public Quaternion m_NetworkRotation;

	public Quaternion GetRotation(Quaternion currentRotation)
	{
		return Quaternion.Lerp(currentRotation, m_NetworkRotation, Time.deltaTime * 3f);
	}

	public void OnPhotonSerializeView(Quaternion currentRotation, PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(currentRotation);
			m_NetworkRotation = currentRotation;
		}
		else
		{
			m_NetworkRotation = (Quaternion)stream.ReceiveNext();
		}
	}
}
