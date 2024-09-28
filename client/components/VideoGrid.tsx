"use client";

import { FC } from "react";
import { VideoCard } from "./VideoCard";
import { Video } from "@/model/video";

interface VideoGridProps {
  videos: Video[];
}

export const VideoGrid: FC<VideoGridProps> = ({ videos }) => {
  return (
    <div className="grid grid-cols-[repeat(auto-fit,minmax(300px,1fr))] gap-x-4 gap-y-6">
      {videos.map((item) => (
        <VideoCard video={item} key={item.video_id} />
      ))}
    </div>
  );
};
