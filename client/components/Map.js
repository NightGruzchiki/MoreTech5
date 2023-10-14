import "leaflet/dist/leaflet.css";
import {
  MapContainer,
  TileLayer,
  Marker,
  Popup,
  ZoomControl,
  LayersControl,
  LayerGroup,
} from "react-leaflet";
import { markerIcon, userIcon } from "./Icon";
import { useState, useRef, useMemo, useEffect } from "react";

function DraggableMarker({ icon, position: markerPosition }) {
  const [lat, lng] = markerPosition
  const [position, setPosition] = useState({ lat, lng });
  useEffect(()=>{
    const [lat, lng] = markerPosition
    setPosition({ lat, lng })
  }, [markerPosition])

  const markerRef = useRef(null);
  const eventHandlers = useMemo(
    () => ({
      dragend() {
        const marker = markerRef.current;
        if (marker != null) {
          setPosition(marker.getLatLng());
        }
      },
    }),
    [],
  );

  return (
    <Marker
      draggable={true}
      eventHandlers={eventHandlers}
      position={position}
      ref={markerRef}
      icon={icon}
    ></Marker>
  );
}

const offices = [
[54.198329, 37.588115],
[54.189933, 37.59391],
[54.160142, 37.586584],
[54.18013, 37.603966],
[54.193144, 37.611458],
[54.19165, 37.617282],
[54.218380, 37.623886],
[54.203047, 37.643686],
[54.196375, 37.679113],
]

const automats = [
    {position:[54.199800, 37.649239],address: 'ул. Кирова, 20'},
    {position:[54.202779, 37.641847],address: 'Ложевая ул., 123'},
    {position:[54.193245, 37.611519],address: 'ул. Демонстрации, 2Г'},
    {position:[54.191522, 37.617549],address: 'Советская улица, 64'},
    {position:[54.199391, 37.636398],address: 'Пролетарская улица, 22А'},
    {position:[54.193431, 37.637050],address: 'Пролетарская улица, 2'},
    {position:[54.208475, 37.669054],address: 'улица Шухова, 26'},
    {position:[54.203102, 37.643743],address: 'Ложевая улица, 125А'},
    {position:[54.213886, 37.619040],address: 'улица Максима Горького, 7А'},
    {position:[54.192951, 37.686278],address: 'ул. Металлургов, 80Г'},
    {position:[54.195850, 37.676193],address: 'улица Металлургов, 65А'},
    {position:[54.196934, 37.605274],address: 'Красноармейский просп., 8А'},
    {position:[54.180026, 37.603911],address: 'проспект Ленина, 77'},
    {position:[54.177997, 37.628743],address: 'Оборонная улица, 85'},
    {position:[54.209401, 37.689692],address: 'ул. Вильямса, 26Б'},
    {position:[54.178070, 37.648626],address: 'улица Аркадия Шипунова, 5'},
    {position:[54.211742, 37.629773],address: 'ул. Максима Горького, 1Б'},
    {position:[54.218385, 37.623975],address: 'ул. Пузакова, 1'},
    {position:[54.185045, 37.697181],address: 'улица Чаплыгина, 7'},
    {position:[54.217163, 37.624858],address: 'Октябрьская ул., 91'},
    {position:[54.204851, 37.684989],address: 'ул. Вильямса, 12'},
    {position:[54.206907, 37.608770],address: 'Комсомольская улица, 3'},
    {position:[54.219667, 37.614783],address: 'ул. Пузакова, 13'},
    {position:[54.211818, 37.693354],address: 'Майская улица, 1'},
    {position:[54.188769, 37.614102],address: 'проспект Ленина, 33'},
    {position:[54.203102, 37.643743],address: 'Ложевая улица, 125А'},
    {position:[54.188769, 37.614102],address: 'просп. Ленина, 33'},
    {position:[54.177997, 37.628743],address: 'Оборонная улица, 85'},
    {position:[54.197879, 37.646534],address: 'ул. Кирова, 19Г'},
    {position:[54.182898, 37.605798],address: 'просп. Ленина, 54'},
    {position:[54.178070, 37.648626],address: 'улица Аркадия Шипунова, 5'},
    {position:[54.177226, 37.600807],address: 'просп. Ленина, 83Б'},
    {position:[54.212578, 37.667545],address: 'ул. Кутузова, 13'},
    {position:[54.178953, 37.634762],address: 'ул. Кауля, 7'},
]

const Map = () => {
  const [userPosition, setUserPosition] = useState([54.195324, 37.620154]);
  useEffect(() => {
    try {
      navigator.geolocation.getCurrentPosition(
        ({ coords: { latitude, longitude } }) =>
          setUserPosition([latitude, longitude]),
      );
    } catch {}
  }, []);
  return (
    <MapContainer
      center={[54.19232, 37.616624]}
      zoom={13}
      scrollWheelZoom={true}
      style={{ height: "100vh", zIndex: "0!important" }}
      zoomControl={false}
    >
      <ZoomControl position="topright" />
      <TileLayer
        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
      />





      <DraggableMarker position={userPosition} icon={userIcon} />

      <LayersControl position="topright">
        <LayersControl.Overlay name="Отделения">
          <LayerGroup>
            {offices.map((position, key)=>(
                <Marker key={key} position={position} icon={markerIcon}>
                  <Popup>Отделение ВТБ</Popup>
                </Marker>
            ))}
          </LayerGroup>
        </LayersControl.Overlay>
        <LayersControl.Overlay checked name="Банкоматы">
          <LayerGroup>
              {automats.map(({position, address}, key)=>(
                  <Marker key={key} position={position} icon={markerIcon}>
                      <Popup>Банкомат ВТБ<br/>{address}</Popup>
                  </Marker>
              ))}
          </LayerGroup>
        </LayersControl.Overlay>
        <LayersControl.Overlay name="Дороги">
          <LayerGroup></LayerGroup>
        </LayersControl.Overlay>
      </LayersControl>
    </MapContainer>
  );
};

export default Map;
