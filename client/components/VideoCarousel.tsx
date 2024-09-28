"use client";

import { Video } from "@/model/video";
import { FC } from "react";
import { Swiper, SwiperSlide } from "swiper/react";
import { VideoCard } from "./VideoCard";
import { A11y, Navigation, Pagination } from "swiper/modules";
import { Button } from "flowbite-react";
import { FaChevronLeft, FaChevronRight } from "react-icons/fa";

import "swiper/css";
import "swiper/css/navigation";
import "swiper/css/pagination";
import "swiper/css/scrollbar";

interface VideoCarouselProps {
  videos: Video[];
}

export const VideoCarousel: FC<VideoCarouselProps> = ({ videos }) => {
  return (
    <div className="relative">
      <Swiper
        className="!pb-9"
        modules={[Navigation, Pagination, A11y]}
        spaceBetween={16}
        slidesPerView={1}
        breakpoints={{
          1280: {
            slidesPerView: 4,
          },
          1024: {
            slidesPerView: 3,
          },
          640: {
            slidesPerView: 2,
          },
        }}
        navigation={{
          nextEl: "#nextEl",
          prevEl: "#prevEl",
        }}
        pagination={{
          clickable: true,
          bulletActiveClass: "swiper-pagination-bullet-active !bg-cyan-500",
        }}
      >
        {videos.map((item) => (
          <SwiperSlide key={item.video_id} className="!h-auto">
            <VideoCard video={item} />
          </SwiperSlide>
        ))}
      </Swiper>
      <Button
        pill
        outline
        id="prevEl"
        className="absolute -left-2 top-1/2 z-[1] hidden size-16 lg:block [&>span]:h-full [&>span]:items-center"
      >
        <FaChevronLeft />
      </Button>
      <Button
        pill
        outline
        id="nextEl"
        className="absolute -right-2 top-1/2 z-[1] hidden size-16 lg:block [&>span]:h-full [&>span]:items-center"
      >
        <FaChevronRight />
      </Button>
    </div>
  );
};
