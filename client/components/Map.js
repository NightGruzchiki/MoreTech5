import "leaflet/dist/leaflet.css";
import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";
import { markerIcon, userIcon } from "./Icon";
import { useState,useRef ,useMemo  } from "react";

function DraggableMarker({ icon, position: [lat,lng]}) {
  const [position, setPosition] = useState({ lat, lng });
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

const Map = () => {
  return (
    <MapContainer
      center={[54.19232, 37.616624]}
      zoom={10}
      scrollWheelZoom={true}
      style={{ height: "100vh", zIndex: "0!important" }}
    >
      <TileLayer
        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
      />

      <Marker position={[54.198329, 37.588115]} icon={markerIcon}>
        <Popup>Отделение ВТБ</Popup>
      </Marker>
      <Marker position={[54.189933, 37.593910]} icon={markerIcon}>
        <Popup>Отделение ВТБ</Popup>
      </Marker>
      <Marker position={[54.160142, 37.586584]} icon={markerIcon}>
        <Popup>Отделение ВТБ</Popup>
      </Marker>
      <Marker position={[54.180130, 37.603966]} icon={markerIcon}>
        <Popup>Отделение ВТБ</Popup>
      </Marker>
      <Marker position={[54.193144, 37.611458]} icon={markerIcon}>
        <Popup>Отделение ВТБ</Popup>
      </Marker>
      <Marker position={[54.191650, 37.617282]} icon={markerIcon}>
        <Popup>Отделение ВТБ</Popup>
      </Marker>

      <DraggableMarker  position={[54.16232, 37.616624]} icon={userIcon}/>
    </MapContainer>
  );
};

export default Map;
