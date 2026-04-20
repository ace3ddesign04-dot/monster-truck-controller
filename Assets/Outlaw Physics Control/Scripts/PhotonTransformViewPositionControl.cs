using UnityEngine;

public class PhotonTransformViewPositionControl
{
	private PhotonTransformViewPositionModel m_Model = new PhotonTransformViewPositionModel();

	public Vector3 m_NetworkPosition;

	public Vector3 UpdatePosition(Vector3 currentPosition)
	{
		Vector3 networkPosition = m_NetworkPosition;
		currentPosition = Vector3.Lerp(currentPosition, networkPosition, Time.deltaTime * 4f);
		if (Vector3.Distance(currentPosition, m_NetworkPosition) > 15f)
		{
			currentPosition = m_NetworkPosition;
		}
		return currentPosition;
	}

	public void OnPhotonSerializeView(Vector3 currentPosition, PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			SerializeData(currentPosition, stream, info);
		}
		else
		{
			DeserializeData(stream, info);
		}
	}

	private void SerializeData(Vector3 currentPosition, PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(currentPosition);
		m_NetworkPosition = currentPosition;
		if (m_Model.ExtrapolateOption == PhotonTransformViewPositionModel.ExtrapolateOptions.SynchronizeValues || m_Model.InterpolateOption == PhotonTransformViewPositionModel.InterpolateOptions.SynchronizeValues)
		{
			stream.SendNext(Vector3.zero);
			stream.SendNext(0f);
		}
	}

	private void DeserializeData(PhotonStream stream, PhotonMessageInfo info)
	{
		Vector3 networkPosition = (Vector3)stream.ReceiveNext();
		if (m_Model.ExtrapolateOption == PhotonTransformViewPositionModel.ExtrapolateOptions.SynchronizeValues || m_Model.InterpolateOption == PhotonTransformViewPositionModel.InterpolateOptions.SynchronizeValues)
		{
			Vector3 vector = (Vector3)stream.ReceiveNext();
			float num = (float)stream.ReceiveNext();
		}
		m_NetworkPosition = networkPosition;
	}
}
