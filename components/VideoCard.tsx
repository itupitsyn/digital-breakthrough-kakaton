"use client";

import { Video } from "@/model/video";
import { Card } from "flowbite-react";
import Image from "next/image";
import { FC, useState } from "react";
import { BiImage } from "react-icons/bi";

interface VideoCardProps {
  video: Video;
}

export const VideoCard: FC<VideoCardProps> = ({ video }) => {
  const [previewError, setPreviewError] = useState(false);

  return (
    <Card
      theme={{
        root: {
          children: "flex h-full flex-col justify-center gap-4",
        },
      }}
    >
      {previewError ? (
        <div className="flex h-36 w-full items-center justify-center rounded-t-lg">
          <BiImage className="size-2/3" />
        </div>
      ) : (
        <Image
          src={video.preview}
          width={512}
          height={512}
          alt="video preview"
          className="h-36 w-full rounded-t-lg object-cover"
          onError={() => setPreviewError(true)}
        />
      )}
      <div className="px-5 pb-5">
        <h5 className="truncate font-medium tracking-tight" title={video.name}>
          {video.name}
        </h5>
      </div>
    </Card>
  );
};
