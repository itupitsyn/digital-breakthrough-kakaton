"use client";

import { Video } from "@/model/video";
import { Card } from "flowbite-react";
import Image from "next/image";
import { FC, useState } from "react";
import { BiArrowToRight, BiImage } from "react-icons/bi";
import { IoMdHeart, IoMdHeartEmpty } from "react-icons/io";
import { TbPoo, TbPooFilled } from "react-icons/tb";

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
      className="[&:hover_.pupsik]:scale-110 "
    >
      <div className="overflow-hidden rounded-t-lg">
        {previewError ? (
          <div className="pupsik flex h-36 w-full items-center justify-center transition-transform">
            <BiImage className="size-2/3" />
          </div>
        ) : (
          <Image
            src={video.preview}
            width={512}
            height={512}
            alt="video preview"
            className="pupsik h-36 w-full object-cover transition-transform"
            onError={() => setPreviewError(true)}
          />
        )}
      </div>
      <div className="flex flex-col gap-2 px-5 pb-5">
        <h5 className="truncate font-medium tracking-tight" title={video.name}>
          {video.name}
        </h5>
        <div className="flex justify-between gap-4">
          <div className="text-sm opacity-80">{new Date(video.date).toLocaleDateString("ru-RU")}</div>
          <div className="flex gap-2 text-cyan-500">
            <button type="button" aria-label="like" title="like">
              {video.state === "like" ? <IoMdHeart className="size-6" /> : <IoMdHeartEmpty className="size-6" />}
            </button>
            <button type="button" aria-label="dislike" title="dislike">
              {video.state === "dislike" ? <TbPooFilled className="size-6" /> : <TbPoo className="size-6" />}
            </button>
            <button type="button" aria-label="skip" title="skip">
              <BiArrowToRight className="size-6" />
            </button>
          </div>
        </div>
      </div>
    </Card>
  );
};
